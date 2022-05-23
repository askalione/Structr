using Xunit;

namespace Structr.Tests.Abstractions.TestsUtils
{
    [CollectionDefinition("FileSystemTests", DisableParallelization = false)]
    public class FileSystemTestsCollection : ICollectionFixture<FileSystemTestFixture>
    {
    }
}
