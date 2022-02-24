using Structr.Operations;

namespace Structr.Samples.Operations.Commands
{
    public interface ICommand<TResult> : IOperation<TResult>
    {
    }

    public interface ICommand : ICommand<VoidResult>, IOperation
    {
    }
}
