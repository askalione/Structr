using System.Collections.Generic;

namespace Structr.Tests.Specifications
{
    public class StringCollection
    {
        public static IEnumerable<string> Items => new List<string> {
            "abc",
            "abcd",
            "abcde",
            "abcdef"
        };

        public static IEnumerable<string> Empty => new List<string>();
    }
}
