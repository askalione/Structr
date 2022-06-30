using Structr.EntityFramework.Options;

namespace Structr.Tests.EntityFramework.Options
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
            result.SoftDeletableFilterName.Should().Be("SoftDeletable");
        }
    }
}
