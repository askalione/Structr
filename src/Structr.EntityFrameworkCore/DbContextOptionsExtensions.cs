using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Structr.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Structr.EntityFrameworkCore
{
    public static class DbContextOptionsExtensions
    {
        public static T GetService<T>(this DbContextOptions options)
        {
            Ensure.NotNull(options, nameof(options));

            var serviceProvider = options.GetExtension<CoreOptionsExtension>().ApplicationServiceProvider;
            return (T)serviceProvider?.GetService(typeof(T));
        }
    }
}
