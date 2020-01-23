using Structr.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Structr.Samples.Data.EFCore.Domain.FooAggregate
{
    public class FooDetail : ValueObject<FooDetail>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
