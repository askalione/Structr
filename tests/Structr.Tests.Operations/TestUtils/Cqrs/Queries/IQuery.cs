using Structr.Operations;

namespace Structr.Tests.Operations.TestUtils.Cqrs.Queries
{
    public interface IQuery<TResult> : IOperation<TResult>
    {
    }
}
