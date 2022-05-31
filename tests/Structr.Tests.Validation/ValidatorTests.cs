using Structr.Tests.Validation.TestUtils.Documents.Contracts;
using Structr.Validation;

namespace Structr.Tests.Validation
{
    public class ValidatorTests
    {
        [Fact]
        public async Task ValidateAsync()
        {
            // Arrange
            var contract = new Contract { Number = "0123456789", Title = "Super long title" };
            var validator = new ContractValidator();

            // Act
            var result = await ((IValidator<Contract>)validator).ValidateAsync(contract, default(CancellationToken));

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
    }
}
