using System.Collections.Generic;

namespace Structr.Tests.Notices.TestUtils
{
    internal class FakeDbContext : IDbContext
    {
        public ICollection<Push> Pushes { get; set; } = new List<Push>();
        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}
