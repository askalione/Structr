using Structr.Operations;
using Structr.Samples.Operations.Commands;
using Structr.Samples.Operations.Queries;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Structr.Samples.Operations.Decorators
{
    public class OperationDecorator<TOperation, TResult> : IOperationHandler<TOperation, TResult>
        where TOperation : IOperation<TResult>
    {
        private readonly IOperationHandler<TOperation, TResult> _handler;
        private readonly IServiceProvider _serviceProvider;

        public OperationDecorator(IOperationHandler<TOperation, TResult> handler,
            IServiceProvider serviceProvider)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }
            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            _handler = handler;
            _serviceProvider = serviceProvider;
        }

        public Task<TResult> HandleAsync(TOperation operation, CancellationToken cancellationToken)
        {
            if (operation is ICommand<TResult>)
            {
                return ((BaseOperationDecorator<TOperation, TResult>)_serviceProvider.GetService(typeof(ICommandDecorator<,>).MakeGenericType(operation.GetType(), typeof(TResult))))
                    .DecorateAsync(operation, _handler, cancellationToken);
            }
            else if (operation is IQuery<TResult>)
            {
                return ((BaseOperationDecorator<TOperation, TResult>)_serviceProvider.GetService(typeof(IQueryDecorator<,>).MakeGenericType(operation.GetType(), typeof(TResult))))
                    .DecorateAsync(operation, _handler, cancellationToken);
            }
            else
            {
                return _handler.HandleAsync(operation, cancellationToken);
            }
        }
    }
}
