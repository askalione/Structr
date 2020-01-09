using Structr.Operations.Internal;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Structr.Operations
{
    public class OperationExecutor : IOperationExecutor
    {
        private readonly IServiceProvider _serviceProvider;
        private static readonly ConcurrentDictionary<Type, object> _cache = new ConcurrentDictionary<Type, object>();

        public OperationExecutor(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
                throw new ArgumentNullException(nameof(serviceProvider));

            _serviceProvider = serviceProvider;
        }

        public Task ExecuteAsync(IOperation operation, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (operation == null)
                throw new ArgumentNullException(nameof(operation));

            var operationType = operation.GetType();

            var handler = _cache.GetOrAdd(operationType,
                type => Activator.CreateInstance(typeof(InternalOperationHandler<>).MakeGenericType(type)));

            return ((InternalHandler)handler).HandleAsync(operation, _serviceProvider, cancellationToken);
        }

        public Task<TResult> ExecuteAsync<TResult>(IOperation<TResult> operation, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (operation == null)
                throw new ArgumentNullException(nameof(operation));

            var operationType = operation.GetType();

            var handler = _cache.GetOrAdd(operationType,
                type => Activator.CreateInstance(typeof(InternalOperationHandler<,>).MakeGenericType(type, typeof(TResult))));

            return ((InternalHandler<TResult>)handler).HandleAsync(operation, _serviceProvider, cancellationToken);
        }
    }
}
