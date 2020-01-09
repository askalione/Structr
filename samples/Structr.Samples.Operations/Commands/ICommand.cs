using Structr.Operations;

namespace Structr.Samples.Operations.Commands
{
    public interface ICommand : IOperation
    {
    }

    public interface ICommand<TResult> : ICommand, IOperation<TResult>
    {
    }
}
