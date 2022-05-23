using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Resources;

namespace Structr.Abstractions.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="Enum"/>.
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Get value of the first <see cref="DescriptionAttribute"/> attribute for the enum element.
        /// </summary>
        /// <param name="value">Element of enumeration.</param>
        /// <returns>Description as string.</returns>
        public static string GetDescription(this Enum value)
        {
            Ensure.NotNull(value, nameof(value));

            if (TryGetAttributes(value, out DescriptionAttribute[] attributes))
            {
                return attributes[0].Description;
            }
            else
            {
                return value.ToString();
            }
        }

        /// <summary>
        /// Get value of first <see cref="DisplayAttribute"/> attribute for the enum element.
        /// </summary>
        /// <param name="value">Element of enumeration.</param>
        /// <returns>Display name as string.</returns>
        public static string GetDisplayName(this Enum value)
        {
            Ensure.NotNull(value, nameof(value));

            if (TryGetAttributes(value, out DisplayAttribute[] attributes))
            {
                var attribute = attributes[0];
                if (attribute.ResourceType != null)
                {
                    var resourceManager = new ResourceManager(attribute.ResourceType.FullName, attribute.ResourceType.Assembly);
                    return resourceManager.GetString(attribute.Name);
                }
                else
                {
                    return attribute.Name;
                }
            }
            else
            {
                return value.ToString();
            }
        }

        private static bool TryGetAttributes<TAttribute>(Enum value, out TAttribute[] attributes)
        {
            return TryGetAttributes(value, false, out attributes);
        }

        private static bool TryGetAttributes<TAttribute>(Enum value, bool inherit, out TAttribute[] attributes)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            attributes = fi.GetCustomAttributes(typeof(TAttribute), inherit) as TAttribute[];
            return attributes != null && attributes.Length > 0;
        }
    }
}
