namespace Structr.Tests.Operations.TestUtils.Cqrs.Queries.Foo
{
    public class BarQuery : IQuery<int>
    {
        public int Number1 { get; set; }
        public int Number2 { get; set; }
    }
}
