using Structr.Operations;
using Structr.Samples.Operations.Commands;
using System.Threading;
using System.Threading.Tasks;

namespace Structr.Samples.Operations.Decorators
{
    public interface ICommandDecorator<TCommand, TResult> where TCommand : ICommand<TResult>
    {
        Task<TResult> DecorateAsync(TCommand command, IOperationHandler<TCommand, TResult> handler, CancellationToken cancellationToken);
    }
}
