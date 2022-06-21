using System.Collections.Concurrent;

namespace Microsoft.EntityFrameworkCore.TestUtilities
{
    /// <remarks>
    /// Taken from https://github.com/dotnet/efcore/blob/main/test/EFCore.Specification.Tests/TestUtilities/TestStoreIndex.cs
    /// </remarks>
    internal class TestStoreIndex
    {
        private readonly HashSet<string> _createdDatabases = new();
        private readonly ConcurrentDictionary<string, object> _creationLocks = new();
        private readonly object _hashSetLock = new();

        public virtual void CreateShared(string name, Action initializeDatabase)
        {
            // ReSharper disable once InconsistentlySynchronizedField
            if (!_createdDatabases.Contains(name))
            {
                var creationLock = _creationLocks.GetOrAdd(name, new object());

                lock (creationLock)
                {
                    if (!_createdDatabases.Contains(name))
                    {
                        initializeDatabase?.Invoke();

                        lock (_hashSetLock)
                        {
                            _createdDatabases.Add(name);
                        }

                        _creationLocks.TryRemove(name, out _);
                    }
                }
            }
        }

        public virtual void RemoveShared(string name)
            => _createdDatabases.Remove(name);

        public virtual void CreateNonShared(string name, Action initializeDatabase)
        {
            var creationLock = _creationLocks.GetOrAdd(name, new object());

            if (Monitor.TryEnter(creationLock))
            {
                try
                {
                    initializeDatabase?.Invoke();
                }
                finally
                {
                    Monitor.Exit(creationLock);
                    if (!_creationLocks.TryRemove(name, out _))
                    {
                        throw new InvalidOperationException(
                            $"An attempt was made to initialize a non-shared store {name} from two different threads.");
                    }
                }
            }
            else
            {
                _creationLocks.TryRemove(name, out _);
                throw new InvalidOperationException(
                    $"An attempt was made to initialize a non-shared store {name} from two different threads.");
            }
        }
    }
}
