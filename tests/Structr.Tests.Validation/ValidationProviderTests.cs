using Microsoft.Extensions.DependencyInjection;
using Structr.Tests.Validation.TestUtils.Documents;
using Structr.Tests.Validation.TestUtils.Documents.Contracts;
using Structr.Validation;

namespace Structr.Tests.Validation
{
    public class ValidationProviderTests
    {
        private IValidationProvider _validationProvider;
        private Contract _contract;

        public ValidationProviderTests()
        {
            var serviceProvider = new ServiceCollection()
                .AddValidation(typeof(Document).Assembly)
                .BuildServiceProvider();
            _validationProvider = serviceProvider.GetRequiredService<IValidationProvider>();

            _contract = new Contract { Number = "0123456789", Title = "Super long title" };
        }

        [Fact]
        public void Ctor()
        {
            // Arrange
            var serviceProvider = new ServiceCollection()
                .BuildServiceProvider();

            // Act
            var result = new ValidationProvider(serviceProvider);

            // Assert
            result.Should().NotBeNull();
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
            // Act
            var result = await _validationProvider.ValidateAsync(_contract);

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
        public async Task ValidateAsync_throws_when_instance_is_null()
        {
            // Act
            Func<Task> act = async () => await _validationProvider.ValidateAsync(null);

            // Assert
            await act.Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task ValidateAndThrowAsync()
        {
            // Act
            Func<Task> act = async () => await _validationProvider.ValidateAndThrowAsync(_contract);

            // Assert
            await act.Should().ThrowExactlyAsync<ValidationException>()
                .WithMessage($"Document number should be 5 characters.{Environment.NewLine}Title is too long.");
        }

        [Fact]
        public async Task ValidateAndThrowAsync_throws_when_instance_is_null()
        {
            // Act
            Func<Task> act = async () => await _validationProvider.ValidateAndThrowAsync(null);

            // Assert
            await act.Should().ThrowExactlyAsync<ArgumentNullException>();
        }
    }
}
