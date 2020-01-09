using System;

namespace Structr.Validation.Internal
{
    internal static class ValidatorProvider
    {
        public static IValidator<T> GetValidator<T>(IServiceProvider serviceProvider)
        {
            var validatorType = typeof(IValidator<T>);
            IValidator<T> validator;

            try
            {
                validator = (IValidator<T>)serviceProvider.GetService(validatorType);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error constructing validator of type {validatorType}", ex);
            }

            if (validator == null)
                validator = new EmptyValidator<T>();

            return validator;
        }
    }
}
