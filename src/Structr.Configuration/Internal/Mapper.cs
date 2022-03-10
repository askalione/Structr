using System;

namespace Structr.Configuration.Internal
{
    internal static class Mapper
    {
        internal static void Map<T>(T source, T destination) where T : class
        {
            if (source == null || destination == null)
            {
                return;
            }

            Type type = typeof(T);

            foreach (var prop in type.GetProperties())
            {
                type.GetProperty(prop.Name).SetValue(destination,
                    type.GetProperty(prop.Name).GetValue(source, null),
                    null);
            }
        }
    }
}
