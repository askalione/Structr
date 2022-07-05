using Microsoft.Extensions.DependencyInjection;
using System;

namespace Structr.Operations
{
    /// <summary>
    /// Allows to configure operation executor service.
    /// </summary>
    public class OperationServiceOptions
    {
        /// <summary>
        /// Changes standard implementation of <see cref="IOperationExecutor"/> to specified one.
        /// </summary>
        public Type ExecutorServiceType { get; set; }

        /// <summary>
        /// Specifies the lifetime of an <see cref="IOperationExecutor"/> service.
        /// </summary>
        public ServiceLifetime ExecutorServiceLifetime { get; set; }

        public OperationServiceOptions()
        {
            ExecutorServiceType = typeof(OperationExecutor);
            ExecutorServiceLifetime = ServiceLifetime.Transient;
        }
    }
}
