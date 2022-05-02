using Structr.Specifications;
using System.Collections.Generic;
using Xunit;

namespace Structr.Tests.Specifications
{
    public class AnySpecificationTests
    {
        [Fact]
        public void IsSatisfiedBy_StringCollection_AllItems()
        {
            var spec = new AnySpecification<string>();

            List<string> actual = new List<string>();
            foreach (var item in StringCollection.Items)
            {
                if (spec.IsSatisfiedBy(item))
                {
                    actual.Add(item);
                }
            }

            Assert.Equal(StringCollection.Items, actual);
        }
    }
}
