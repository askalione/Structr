namespace Structr.Tests.Email.TestUtils
{
    internal static class TestDataPath
    {
        public static string ContentRootPath => Path.Combine(
            Directory.GetParent(Directory.GetCurrentDirectory())!.Parent!.Parent!.FullName,
            "TestData");

        public static string Combine(string path)
            => Path.Combine(ContentRootPath, path);
    }
}
