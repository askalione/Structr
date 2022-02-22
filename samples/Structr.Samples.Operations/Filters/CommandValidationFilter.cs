using Structr.Operations;
using Structr.Samples.Operations.Commands;
using Structr.Validation;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Structr.Samples.Operations.Filters
{
    public abstract class CommandValidationFilter
    {
        private readonly IValidationProvider _validationProvider;

        protected CommandValidationFilter(IValidationProvider validationProvider)
        {
            if (validationProvider == null)
            {
                throw new ArgumentNullException(nameof(validationProvider));
            }

            _validationProvider = validationProvider;
        }

        protected Task ValidateAsync<TCommand>(TCommand command, CancellationToken cancellationToken)
            where TCommand : ICommand
            => _validationProvider.ValidateAndThrowAsync(command, cancellationToken);
    }

    public class CommandValidationFilter<TCommand> : CommandValidationFilter, IOperationFilter<TCommand>
        where TCommand : ICommand
    {
        public CommandValidationFilter(IValidationProvider validationProvider)
            : base(validationProvider)
        { }

        public async Task HandleAsync(TCommand command, CancellationToken cancellationToken, OperationHandlerDelegate next)
        {
            await ValidateAsync(command, cancellationToken);
            await next();
        }
    }

    public class CommandValidationFilter<TCommand, TResult> : CommandValidationFilter, IOperationFilter<TCommand, TResult>
        where TCommand : ICommand<TResult>
    {
        public CommandValidationFilter(IValidationProvider validationProvider)
            : base(validationProvider)
        { }

        public async Task<TResult> HandleAsync(TCommand command, CancellationToken cancellationToken, OperationHandlerDelegate<TResult> next)
        {
            await ValidateAsync(command, cancellationToken);
            return await next();
        }
    }
}
