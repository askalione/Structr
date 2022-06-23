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
            // Act
            var serviceProvider = new ServiceCollection()
                .AddValidation(typeof(Document).Assembly)
                .BuildServiceProvider();

            // Assert
            serviceProvider.GetService<IValidationProvider>()
                .Should().BeOfType<ValidationProvider>();
            serviceProvider.GetService<IValidator<Contract>>()
                .Should().BeOfType<ContractValidator>();
        }

        [Fact]
        public void AddValidation_adds_scoped_provider()
        {
            // Act
            var serviceProvider = new ServiceCollection()
                .AddValidation(typeof(Document).Assembly)
                .BuildServiceProvider();

            // Assert
            var validationProvider1 = serviceProvider.GetRequiredService<IValidationProvider>();
            var validationProvider2 = serviceProvider.GetRequiredService<IValidationProvider>();
            validationProvider1.Should().Be(validationProvider2);
        }

        [Fact]
        public void AddValidation_with_setting_custom_provider()
        {
            // Act
            var serviceProvider = new ServiceCollection()
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
            // Act
            var serviceProvider = new ServiceCollection()
                .AddValidation(options =>
                {
                    options.Lifetime = ServiceLifetime.Transient;
                },
                typeof(Document).Assembly)
                .BuildServiceProvider();

            // Assert
            var validationProvider1 = serviceProvider.GetRequiredService<IValidationProvider>();
            var validationProvider2 = serviceProvider.GetRequiredService<IValidationProvider>();
            validationProvider1.Should().NotBe(validationProvider2);
        }
    }
}
