using Structr.Domain;

namespace Structr.Tests.Domain.TestUtils.Users
{
    public class Address : ValueObject<Address>
    {
        public string Town { get; set; }
        public string Street { get; set; }
        public string House { get; set; }
    }
}
