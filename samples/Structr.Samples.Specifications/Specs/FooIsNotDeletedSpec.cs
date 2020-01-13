using Structr.Samples.Specifications.Models;
using Structr.Specifications;
using System;
using System.Linq.Expressions;

namespace Structr.Samples.Specifications.Specs
{
    public class FooIsNotDeletedSpec : Specification<Foo>
    {
        public override Expression<Func<Foo, bool>> ToExpression()
        {
            return x => x.DateDeleted == null;
        }
    }
}
