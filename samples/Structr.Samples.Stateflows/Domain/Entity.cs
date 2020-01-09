using System;
using System.Collections.Generic;
using System.Text;

namespace Structr.Samples.Stateflows.Domain
{
    public abstract class Entity
    {
        public Guid Id { get; protected set; }

        protected Entity()
        {
            Id = Guid.NewGuid();
        }
    }
}
