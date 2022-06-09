using System;

namespace Structr.Configuration.Internal
{
    /// <summary>
    /// Class for mapping.
    /// </summary>
    internal static class Mapper
    {
        /// <summary>
        /// Map source to destination.
        /// </summary>
        /// <typeparam name="T">Any class.</typeparam>
        /// <param name="source">Instance of T.</param>
        /// <param name="destination">Instance of T.</param>
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
