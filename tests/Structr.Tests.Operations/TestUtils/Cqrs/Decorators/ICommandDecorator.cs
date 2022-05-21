using Structr.Operations;
using System.Threading;
using System.Threading.Tasks;

namespace Structr.Tests.Operations.TestUtils.Cqrs.Decorators
{
    public interface ICommandDecorator<TCommand, TResult> where TCommand : IOperation<TResult>
    {
        Task<TResult> DecorateAsync(TCommand command, IOperationHandler<TCommand, TResult> handler, CancellationToken cancellationToken);
    }
}
