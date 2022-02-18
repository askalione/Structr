using Microsoft.Extensions.DependencyInjection;
using System;

namespace Structr.Operations
{
    public class OperationServiceOptions
    {
        public Type ExecutorType { get; set; }
        public ServiceLifetime ExecutorServiceLifetime { get; set; }

        public OperationServiceOptions()
        {
            ExecutorType = typeof(OperationExecutor);
            ExecutorServiceLifetime = ServiceLifetime.Scoped;
        }
    }
}
