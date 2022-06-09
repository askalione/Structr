using Xunit;

namespace Structr.Tests.Configuration.TestUtils
{
    [CollectionDefinition("Tests with temp files", DisableParallelization = false)]
    public class TestsWithTempFilesCollection : ICollectionFixture<TestsWithTempFilesFixture>
    { }
}
