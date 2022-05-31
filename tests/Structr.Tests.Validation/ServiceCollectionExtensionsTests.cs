using Microsoft.Extensions.DependencyInjection;
using Structr.Tests.Validation.TestUtils;
using Structr.Tests.Validation.TestUtils.Documents;
using Structr.Tests.Validation.TestUtils.Documents.Contracts;
using Structr.Validation;

namespace Structr.Tests.Validation
{
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void AddValidation()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            var serviceProvider = services
                .AddValidation(typeof(Document).Assembly)
                .BuildServiceProvider();

            // Assert
            serviceProvider.GetService<IValidationProvider>()
                .Should().BeOfType<ValidationProvider>();
            serviceProvider.GetService<IValidator<Contract>>()
                .Should().BeOfType<ContractValidator>();
        }

        [Fact]
        public void AddValidation_with_setting_custom_provider()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            var serviceProvider = services
                .AddValidation(options =>
                {
                    options.ProviderType = typeof(CustomValidationProvider);
                },
                typeof(Document).Assembly)
                .BuildServiceProvider();

            // Assert
            serviceProvider.GetService<IValidationProvider>()
                .Should().BeOfType<CustomValidationProvider>();
        }

        [Fact]
        public void AddValidation_with_setting_lifetime()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            var serviceProvider = services
                .AddValidation(options =>
                {
                    options.Lifetime = ServiceLifetime.Transient;
                },
                typeof(Document).Assembly)
                .BuildServiceProvider();

            // Assert
            var validationProvider1 = serviceProvider.GetRequiredService<IValidationProvider>();
            var validationProvider2 = serviceProvider.GetRequiredService<IValidationProvider>();
            validationProvider1.Equals(validationProvider2).Should().BeFalse();
        }
    }
}
