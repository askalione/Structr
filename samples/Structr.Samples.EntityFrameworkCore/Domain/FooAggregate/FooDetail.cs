using Structr.Domain;

namespace Structr.Samples.EntityFrameworkCore.Domain.FooAggregate
{
    public class FooDetail : ValueObject<FooDetail>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
