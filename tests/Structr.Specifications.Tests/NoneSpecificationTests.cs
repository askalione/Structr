using System.Collections.Generic;
using Xunit;

namespace Structr.Specifications.Tests
{
    public class NoneSpecificationTests
    {
        [Fact]
        public void IsSatisfiedBy_StringCollection_NoneItems()
        {
            var spec = new NoneSpecification<string>();

            List<string> actual = new List<string>();
            foreach (var item in StringCollection.Items)
            {
                if (spec.IsSatisfiedBy(item))
                {
                    actual.Add(item);
                }
            }

            Assert.Equal(StringCollection.Empty, actual);
        }
    }
}
