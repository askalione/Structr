using Microsoft.Extensions.DependencyInjection.Extensions;
using Structr.Email;
using Structr.Email.Razor;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class EmailServiceBuilderExtensions
    {
        public static EmailServiceBuilder AddRazorTemplateRenderer(this EmailServiceBuilder builder)
        {
            builder.Services.TryAddSingleton<IEmailTemplateRenderer, RazorEmailTemplateRenderer>();

            return builder;
        }
    }
}
