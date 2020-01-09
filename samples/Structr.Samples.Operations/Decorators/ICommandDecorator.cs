using Structr.Operations;
using System.Threading;
using System.Threading.Tasks;

namespace Structr.Samples.Operations.Decorators
{
    public interface ICommandDecorator<TCommand> where TCommand : IOperation
    {
        Task DecorateAsync(TCommand command, IOperationHandler<TCommand> handler, CancellationToken cancellationToken);
    }

    public interface ICommandDecorator<TCommand, TResult> where TCommand : IOperation<TResult>
    {
        Task<TResult> DecorateAsync(TCommand command, IOperationHandler<TCommand, TResult> handler, CancellationToken cancellationToken);
    }
}
