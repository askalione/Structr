using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;

namespace Structr.EntityFrameworkCore
{
    public static class DbContextOptionsExtensions
    {
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
