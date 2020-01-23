using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using Structr.Abstractions;

namespace Structr.Data.EFCore
{
    public static class DbContextOptionsBuilderExtensions
    {
        public static DbContextOptionsBuilder UseLoggerProvider(this DbContextOptionsBuilder builder, ILoggerProvider provider)
        {
            Ensure.NotNull(builder, nameof(builder));
            Ensure.NotNull(provider, nameof(provider));

            var extension = builder.Options.FindExtension<CoreOptionsExtension>();
            var loggerFactory = extension?.LoggerFactory;
            if (extension.LoggerFactory != null)
            {
                loggerFactory.AddProvider(provider);
            }
            else
            {
                builder.UseLoggerFactory(new LoggerFactory(new ILoggerProvider[] {
                    provider
                }));
            }

            return builder;
        }
    }
}
