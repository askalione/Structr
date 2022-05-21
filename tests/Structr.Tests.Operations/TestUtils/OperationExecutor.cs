using Structr.Operations;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Structr.Tests.Operations.TestUtils.Cqrs
{
    public class FakeExecutor : IOperationExecutor
    {        
        public async Task<TResult> ExecuteAsync<TResult>(IOperation<TResult> operation, CancellationToken cancellationToken = default)
        {
            return await new Task<TResult>(() => Activator.CreateInstance<TResult>());
        }
    }
}
