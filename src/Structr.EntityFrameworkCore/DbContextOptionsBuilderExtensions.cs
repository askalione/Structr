using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using System;

namespace Structr.EntityFrameworkCore
{
    /// <summary>
    /// Extensions for the <see cref="DbContextOptionsBuilder"/>.
    /// </summary>
    public static class DbContextOptionsBuilderExtensions
    {
        /// <summary>
        /// Adds a logger to a <see cref="DbContextOptionsBuilder"/> instance.
        /// </summary>
        /// <param name="builder">The <see cref="DbContextOptionsBuilder"/>.</param>
        /// <param name="provider">The <see cref="ILoggerProvider"/>.</param>
        /// <returns>The <see cref="DbContextOptionsBuilder"/>.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="builder"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">If <paramref name="provider"/> is <see langword="null"/>.</exception>
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
