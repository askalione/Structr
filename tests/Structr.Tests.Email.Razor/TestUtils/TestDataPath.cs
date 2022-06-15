namespace Structr.Tests.Email.Razor.TestUtils
{
    internal static class TestDataPath
    {
        public static string ContentRootPath => Path.Combine(
            Directory.GetParent(Directory.GetCurrentDirectory())!.Parent!.Parent!.FullName,
            "TestData");

        public static string Combine(string fileName)
            => Path.Combine(ContentRootPath, fileName);
    }
}
