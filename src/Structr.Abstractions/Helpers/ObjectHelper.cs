namespace Structr.Abstractions.Helpers
{
    public static class ObjectHelper
    {
        public static string Dump(object instance, int depth = 4, int indentSize = 2, char indentChar = ' ')
        {
            var dumper = new ObjectDumper(depth, indentSize, indentChar);
            return dumper.Dump(instance, isTopOfTree: true);
        }
    }
}
