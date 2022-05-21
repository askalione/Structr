using Structr.Operations;

namespace Structr.Tests.Operations.TestUtils.Cqrs.Commands
{
    public interface ICommand<TResult> : IOperation<TResult>
    {
    }

    public interface ICommand : ICommand<VoidResult>, IOperation
    {
    }
}
