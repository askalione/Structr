using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Resources;

namespace Structr.Abstractions.Extensions
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Get value of first description attribute for enum element.
        /// </summary>
        /// <param name="value">Element of enumeration.</param>
        /// <returns>Return description as string.</returns>
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
        /// Get value of first display name attribute for enum element.
        /// </summary>
        /// <param name="value">Element of enumeration.</param>
        /// <returns></returns>
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
