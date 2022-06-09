using Microsoft.Extensions.DependencyInjection.Extensions;
using Structr.Email;
using Structr.Email.Razor;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods for <see cref="EmailServiceBuilder"/>.
    /// </summary>
    public static class EmailServiceBuilderExtensions
    {
        /// <summary>
        /// Adds <see cref="RazorEmailTemplateRenderer"/> service to <see cref="EmailServiceBuilder"/> services.
        /// </summary>
        /// <param name="builder">The <see cref="EmailServiceBuilder"/>.</param>
        /// <returns>The <see cref="EmailServiceBuilder"/>.</returns>
        public static EmailServiceBuilder AddRazorTemplateRenderer(this EmailServiceBuilder builder)
        {
            builder.Services.TryAddSingleton<IEmailTemplateRenderer, RazorEmailTemplateRenderer>();

            return builder;
        }
    }
}
