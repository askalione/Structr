using Microsoft.Extensions.DependencyInjection;
using Structr.Validation;

namespace Structr.Tests.Validation
{
    public class ValidationServiceOptionsTests
    {
        [Fact]
        public void Ctor()
        {
            // Act
            var result = new ValidationServiceOptions();

            // Assert
            result.ProviderServiceType.Should().Be(typeof(ValidationProvider));
            result.ProviderServiceLifetime.Should().Be(ServiceLifetime.Transient);
        }
    }
}
