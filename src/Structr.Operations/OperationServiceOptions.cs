using Microsoft.Extensions.DependencyInjection;
using System;

namespace Structr.Operations
{
    public class OperationServiceOptions
    {
        public Type ExecutorType { get; set; }
        public ServiceLifetime Lifetime { get; set; }

        public OperationServiceOptions()
        {
            ExecutorType = typeof(OperationExecutor);
            Lifetime = ServiceLifetime.Scoped;
        }
    }
}
