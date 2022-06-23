using System.Collections.Generic;

namespace Structr.Tests.Notices.TestUtils
{
    internal interface IDbContext
    {
        ICollection<Push> Pushes { get; }
        ICollection<Ticket> Tickets { get; }
    }
}
