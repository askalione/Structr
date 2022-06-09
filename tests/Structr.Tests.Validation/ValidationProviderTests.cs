using Microsoft.Extensions.DependencyInjection;
using Structr.Tests.Validation.TestUtils.Documents;
using Structr.Tests.Validation.TestUtils.Documents.Bills;
using Structr.Tests.Validation.TestUtils.Documents.Contracts;
using Structr.Validation;

namespace Structr.Tests.Validation
{
    public class ValidationProviderTests
    {
        [Fact]
        public void Ctor()
        {
            // Arrange
            var serviceProvider = new ServiceCollection()
                .BuildServiceProvider();

            // Act
            Action act = () => new ValidationProvider(serviceProvider);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void Ctor_throws_when_serviceProvider_is_null()
        {
            // Act
            Action act = () => new ValidationProvider(null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public async Task ValidateAsync()
        {
            // Arrange
            var validationProvider = GetValidationProvider();
            var contract = new Contract { Number = "0123456789", Title = "Super long title" };

            // Act
            var result = await validationProvider.ValidateAsync(contract);

            // Assert
            result.Should().SatisfyRespectively(
                first =>
                {
                    first.Message.Should().Be("Document number should be 5 characters.");
                    first.ParameterName.Should().Be("Number");
                    first.ActualValue.Should().Be("0123456789");
                },
                second =>
                {
                    second.Message.Should().Be("Title is too long.");
                    second.ParameterName.Should().Be("Title");
                    second.ActualValue.Should().Be("Super long title");
                });
        }

        [Fact]
        public async Task ValidateAsync_when_no_validator_is_presented()
        {
            // Arrange
            var validationProvider = GetValidationProvider();
            var contract = new Bill { Number = "0123456789", Price = 100 };

            // Act
            var result = await validationProvider.ValidateAsync(contract);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public async Task ValidateAsync_throws_when_instance_is_null()
        {
            // Arrange
            var validationProvider = GetValidationProvider();

            // Act
            Func<Task> act = async () => await validationProvider.ValidateAsync(null);

            // Assert
            await act.Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task ValidateAndThrowAsync()
        {
            // Arrange
            var validationProvider = GetValidationProvider();
            var contract = new Contract { Number = "0123456789", Title = "Super long title" };

            // Act
            Func<Task> act = async () => await validationProvider.ValidateAndThrowAsync(contract);

            // Assert
            await act.Should().ThrowExactlyAsync<ValidationException>()
                .WithMessage($"Document number should be 5 characters.{Environment.NewLine}Title is too long.");
        }

        [Fact]
        public async Task ValidateAndThrowAsync_throws_when_instance_is_null()
        {
            // Arrange
            var validationProvider = GetValidationProvider();

            // Act
            Func<Task> act = async () => await validationProvider.ValidateAndThrowAsync(null);

            // Assert
            await act.Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        private IValidationProvider GetValidationProvider()
        {
            var serviceProvider = new ServiceCollection()
                .AddValidation(typeof(Document).Assembly)
                .BuildServiceProvider();
            return serviceProvider.GetRequiredService<IValidationProvider>();
        }
    }
}
