using Structr.Samples.Specifications.Models;
using Structr.Specifications;
using System;
using System.Linq.Expressions;

namespace Structr.Samples.Specifications.Specs
{
    public class FooNameContainsTextSpec : Specification<Foo>
    {
        public string Text { get; }

        public FooNameContainsTextSpec(string text)
        {
            if (string.IsNullOrEmpty(text))
                throw new ArgumentNullException(nameof(text));

            Text = text;
        }

        public override Expression<Func<Foo, bool>> ToExpression()
        {
            return x => x.Name.ToUpper().Contains(Text.ToUpper());
        }
    }
}
