using Microsoft.Extensions.DependencyInjection.Extensions;
using Structr.Email;
using Structr.Email.Razor;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class EmailServiceBuilderExtensions
    {
        public static EmailServiceBuilder AddRazorTemplateRenderer(this EmailServiceBuilder builder, string path)
            => AddRazorTemplateRenderer(builder, _ => path);

        public static EmailServiceBuilder AddRazorTemplateRenderer(this EmailServiceBuilder builder, Func<IServiceProvider, string> pathFactory)
        {
            builder.Services.TryAddSingleton<IEmailTemplateRenderer>(serviceProvider => new RazorEmailTemplateRenderer(pathFactory(serviceProvider)));

            return builder;
        }
    }
}
