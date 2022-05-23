using System;
using System.Linq;

namespace Structr.Abstractions.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="object"/>.
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Sets property of source instance to specified value.
        /// When property with provided name doesn't exist then nothing happends.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <param name="propertyName">Property name.</param>
        /// <param name="propertyValue">Property value.</param>
        public static void SetProperty<T>(this T instance, string propertyName, object propertyValue)
        {
            Ensure.NotNull(instance, nameof(instance));
            Ensure.NotEmpty(propertyName, nameof(propertyName));

            bool propertyHasDot = propertyName.IndexOf(".") > -1;
            string firstPartBeforeDot;
            string nextParts = "";

            if (propertyHasDot == false)
            {
                firstPartBeforeDot = propertyName.ToLower();
                var property = instance.GetType()
                    .GetProperties()
                    .FirstOrDefault(x => x.Name.Equals(firstPartBeforeDot, StringComparison.OrdinalIgnoreCase));
                if (property != null)
                {
                    var propertyType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                    property.SetValue(instance, propertyValue != null ? Convert.ChangeType(propertyValue, propertyType) : propertyValue, null);
                }
            }
            else
            {
                firstPartBeforeDot = propertyName.Substring(0, propertyName.IndexOf(".")).ToLower();
                nextParts = propertyName.Substring(propertyName.IndexOf(".") + 1);
                var childObject = instance.GetType()
                    .GetProperties()
                    .FirstOrDefault(x => x.Name.Equals(firstPartBeforeDot, StringComparison.OrdinalIgnoreCase))?.GetValue(instance, null);
                if (childObject != null)
                {
                    SetProperty(childObject, nextParts, propertyValue);
                }
            }
        }

        /// <summary>
        /// Dumps all object's properties into string for specified count of levels depth.
        /// </summary>
        /// <param name="instance">Object to dump.</param>
        /// <param name="depth">Scaning depth. 1 - means object itself. 2 - object's properties. 3 - properties of nested objects, etc.</param>
        /// <param name="indentSize">Number of intend chars.</param>
        /// <param name="indentChar">Char using to indent lines.</param>
        /// <returns>Object's data dump.</returns>
        public static string Dump(this object instance, int depth = 4, int indentSize = 2, char indentChar = ' ')
        {
            var dumper = new ObjectDumper(depth, indentSize, indentChar);
            return dumper.Dump(instance, isTopOfTree: true);
        }
    }
}
