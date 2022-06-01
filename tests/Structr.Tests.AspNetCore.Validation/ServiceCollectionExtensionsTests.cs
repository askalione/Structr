#nullable disable

using FluentAssertions;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Structr.Tests.AspNetCore.Validation
{
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void AddAspNetCoreValidation()
        {
            // Act
            var servicesProvider = new ServiceCollection()
                .AddAspNetCoreValidation()
                .BuildServiceProvider();

            // Assert
            var result = servicesProvider.GetService<IValidationAttributeAdapterProvider>();
            result.Should().NotBeNull();
        }
    }
}