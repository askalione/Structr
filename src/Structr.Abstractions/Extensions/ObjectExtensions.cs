using System;
using System.Linq;

namespace Structr.Abstractions.Extensions
{
    public static class ObjectExtensions
    {
        public static void SetProperty<T>(this T instance, string propertyName, object propertyValue)
        {
            Ensure.NotNull(instance, nameof(instance));
            Ensure.NotEmpty(propertyName, nameof(propertyName));

            bool propertyHasDot = propertyName.IndexOf(".") > -1;
            string firstPartBeforeDot;
            string nextParts = "";

            if (!propertyHasDot)
            {
                firstPartBeforeDot = propertyName.ToLower();
                var property = instance.GetType()
                    .GetProperties()
                    .FirstOrDefault(x => x.Name.Equals(firstPartBeforeDot, StringComparison.OrdinalIgnoreCase));
                if (property != null)
                    property.SetValue(instance, Convert.ChangeType(propertyValue, property.PropertyType), null);
            }
            else
            {
                firstPartBeforeDot = propertyName.Substring(0, propertyName.IndexOf(".")).ToLower();
                nextParts = propertyName.Substring(propertyName.IndexOf(".") + 1);
                var childObject = instance.GetType()
                    .GetProperties()
                    .FirstOrDefault(x => x.Name.Equals(firstPartBeforeDot, StringComparison.OrdinalIgnoreCase))?.GetValue(instance, null);
                if (childObject != null)
                    SetProperty(childObject, nextParts, propertyValue);
            }
        }
    }
}
