using Structr.Domain;
using Structr.Tests.Domain.TestUtils.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Structr.Tests.Domain.TestUtils.Items
{
    // Wrong TEntity generic parameter specified
    internal class Item : Entity<User>
    {
        public override bool Equals(User other)
        {
            throw new NotImplementedException();
        }

        public override bool IsTransient()
        {
            throw new NotImplementedException();
        }

        protected override int GenerateHashCode()
        {
            throw new NotImplementedException();
        }
    }
}
