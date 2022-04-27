using Structr.Samples.Specifications.Models;
using Structr.Specifications;
using System;
using System.Linq.Expressions;

namespace Structr.Samples.Specifications.Specs
{
    public class FooIsLightAndYoungerSpec : Specification<Foo>
    {
        public int Age { get; }

        public FooIsLightAndYoungerSpec(int age)
        {
            Age = age;
        }

        public override Expression<Func<Foo, bool>> ToExpression()
        {
            return x => x.Age <= Age && x.Color != Color.Black;
        }
    }
}
