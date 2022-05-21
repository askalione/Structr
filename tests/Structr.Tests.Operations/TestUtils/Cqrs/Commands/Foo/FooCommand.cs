using Structr.Operations;

namespace Structr.Tests.Operations.TestUtils.Cqrs.Commands.Foo
{
    public class FooCommand : ICommand
    {
        public string? Name { get; set; }
    }
}
