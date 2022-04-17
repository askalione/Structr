using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using System;

namespace Structr.EntityFrameworkCore
{
    public static class DbContextOptionsBuilderExtensions
    {
        public static DbContextOptionsBuilder UseLoggerProvider(this DbContextOptionsBuilder builder, ILoggerProvider provider)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }

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
