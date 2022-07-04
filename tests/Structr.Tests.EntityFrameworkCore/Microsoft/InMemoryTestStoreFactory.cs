using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.EntityFrameworkCore.TestUtilities
{
    /// <remarks>
    /// Taken from https://github.com/dotnet/efcore/blob/main/test/EFCore.InMemory.FunctionalTests/TestUtilities/InMemoryTestStoreFactory.cs
    /// </remarks>
    internal class InMemoryTestStoreFactory : ITestStoreFactory
    {
        public static InMemoryTestStoreFactory Instance { get; } = new();

        protected InMemoryTestStoreFactory()
        {
        }

        //public TestStore Create(string storeName)
        //    => InMemoryTestStore.Create(storeName);

        //public TestStore GetOrCreate(string storeName)
        //    => InMemoryTestStore.GetOrCreate(storeName);

        public IServiceCollection AddProviderServices(IServiceCollection serviceCollection)
            => serviceCollection.AddEntityFrameworkInMemoryDatabase()
                .AddSingleton<TestStoreIndex>();

        //public ListLoggerFactory CreateListLoggerFactory(Func<string, bool> shouldLogCategory)
        //    => new(shouldLogCategory);
    }
}
