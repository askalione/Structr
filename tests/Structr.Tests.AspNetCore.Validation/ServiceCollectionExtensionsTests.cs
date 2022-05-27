#nullable disable

using Xunit;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.DataAnnotations;

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