using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.EntityFrameworkCore.TestUtilities
{
    /// <remarks>
    /// Taken from https://github.com/dotnet/efcore/blob/main/test/EFCore.Specification.Tests/TestUtilities/ITestStoreFactory.cs
    /// </remarks>
    internal interface ITestStoreFactory
    {
        //TestStore Create(string storeName);
        //TestStore GetOrCreate(string storeName);
        IServiceCollection AddProviderServices(IServiceCollection serviceCollection);
        //ListLoggerFactory CreateListLoggerFactory(Func<string, bool> shouldLogCategory);
    }
}
