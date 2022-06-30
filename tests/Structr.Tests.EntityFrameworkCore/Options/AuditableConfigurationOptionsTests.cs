using Structr.EntityFrameworkCore.Options;

namespace Structr.Tests.EntityFrameworkCore.Options
{
    public class AuditableConfigurationOptionsTests
    {
        [Fact]
        public void Ctor()
        {
            // Act
            var result = new AuditableConfigurationOptions();

            // Assert
            result.SignedColumnMaxLength.Should().Be(50);
            result.SignedColumnIsRequired.Should().BeFalse();
        }
    }
}
