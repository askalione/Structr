using Structr.Validation;

namespace Structr.Tests.Validation.TestUtils
{
    internal class CustomValidationProvider : ValidationProvider
    {
        public CustomValidationProvider(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}
