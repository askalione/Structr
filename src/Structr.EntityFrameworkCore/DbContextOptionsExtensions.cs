using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;

namespace Structr.EntityFrameworkCore
{
    /// <summary>
    /// Extensions for the <see cref="DbContextOptions"/>.
    /// </summary>
    public static class DbContextOptionsExtensions
    {
        /// <summary>
        /// Returns service <see cref="{T}"/>.
        /// </summary>
        /// <typeparam name="T">Service type.</typeparam>
        /// <param name="options">The <see cref="DbContextOptions"/>.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="options"/> is <see langword="null"/>.</exception>
        public static T GetService<T>(this DbContextOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            var serviceProvider = options.GetExtension<CoreOptionsExtension>().ApplicationServiceProvider;
            return (T)serviceProvider?.GetService(typeof(T));
        }
    }
}
