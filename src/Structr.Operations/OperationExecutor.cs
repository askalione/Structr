using Structr.Operations.Internal;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Structr.Operations
{
    /// <summary>
    /// Default implementation of operation executor.
    /// </summary>
    public class OperationExecutor : IOperationExecutor
    {
        private readonly IServiceProvider _serviceProvider;
        private static readonly ConcurrentDictionary<Type, object> _cache = new ConcurrentDictionary<Type, object>();

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationExecutor"/>.
        /// </summary>
        /// <param name="serviceProvider">The <see cref="IServiceProvider"/>.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
        public OperationExecutor(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            _serviceProvider = serviceProvider;
        }

        public Task<TResult> ExecuteAsync<TResult>(IOperation<TResult> operation, CancellationToken cancellationToken = default)
        {
            if (operation == null)
            {
                throw new ArgumentNullException(nameof(operation));
            }

            var operationType = operation.GetType();

            var handler = _cache.GetOrAdd(operationType,
                type => Activator.CreateInstance(typeof(InternalOperationHandler<,>).MakeGenericType(type, typeof(TResult))));

            return ((InternalHandler<TResult>)handler).HandleAsync(operation, _serviceProvider, cancellationToken);
        }
    }
}
