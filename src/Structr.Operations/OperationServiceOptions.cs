using Microsoft.Extensions.DependencyInjection;
using System;

namespace Structr.Operations
{
    /// <summary>
    /// Allows to configure operation executor service.
    /// </summary>
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
