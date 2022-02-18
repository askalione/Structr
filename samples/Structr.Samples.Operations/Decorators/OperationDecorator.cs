using Structr.Operations;
using Structr.Samples.Operations.Commands;
using Structr.Samples.Operations.Queries;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Structr.Samples.Operations.Decorators
{
    public class OperationDecorator<TOperation> : IOperationHandler<TOperation> where TOperation : IOperation
    {
        private readonly IOperationHandler<TOperation> _handler;
        private readonly ICommandDecorator<TOperation> _commandDecorator;

        public OperationDecorator(IOperationHandler<TOperation> handler, ICommandDecorator<TOperation> commandDecorator)
        {
            if (commandDecorator == null)
                throw new ArgumentNullException(nameof(commandDecorator));
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            _handler = handler;
            _commandDecorator = commandDecorator;
        }

        public Task HandleAsync(TOperation operation, CancellationToken cancellationToken)
        {
            return _commandDecorator.DecorateAsync(operation, _handler, cancellationToken);
        }
    }

    public class OperationDecorator<TOperation, TResult> : IOperationHandler<TOperation, TResult> where TOperation : IOperation<TResult>
    {
        private readonly IOperationHandler<TOperation, TResult> _handler;
        private readonly ICommandDecorator<TOperation, TResult> _commandDecorator;
        private readonly IQueryDecorator<TOperation, TResult> _queryDecorator;

        public OperationDecorator(IOperationHandler<TOperation, TResult> handler,
            ICommandDecorator<TOperation, TResult> commandDecorator,
            IQueryDecorator<TOperation, TResult> queryDecorator)
        {
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));
            if (commandDecorator == null)
                throw new ArgumentNullException(nameof(commandDecorator));
            if (queryDecorator == null)
                throw new ArgumentNullException(nameof(queryDecorator));

            _handler = handler;
            _commandDecorator = commandDecorator;
            _queryDecorator = queryDecorator;
        }

        public Task<TResult> HandleAsync(TOperation operation, CancellationToken cancellationToken)
        {
            if (operation is ICommand)
                return _commandDecorator.DecorateAsync(operation, _handler, cancellationToken);
            else if (operation is IQuery<TResult>)
                return _queryDecorator.DecorateAsync(operation, _handler, cancellationToken);
            else
                return _handler.HandleAsync(operation, cancellationToken);
        }
    }
}
