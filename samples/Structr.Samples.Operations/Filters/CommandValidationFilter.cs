using Structr.Operations;
using Structr.Samples.Operations.Commands;
using Structr.Validation;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Structr.Samples.Operations.Filters
{
    public class CommandValidationFilter<TCommand, TResult> : IOperationFilter<TCommand, TResult>
         where TCommand : ICommand<TResult>
    {
        private readonly IValidationProvider _validationProvider;

        public CommandValidationFilter(IValidationProvider validationProvider)
        {
            if (validationProvider == null)
            {
                throw new ArgumentNullException(nameof(validationProvider));
            }

            _validationProvider = validationProvider;
        }

        public async Task<TResult> FilterAsync(TCommand command, CancellationToken cancellationToken, OperationHandlerDelegate<TResult> next)
        {
            await _validationProvider.ValidateAndThrowAsync(command, cancellationToken);
            return await next();
        }
    }
}
