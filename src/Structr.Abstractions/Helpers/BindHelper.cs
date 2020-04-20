using Structr.Abstractions.Attributes;
using Structr.Abstractions.Extensions;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Structr.Abstractions.Helpers
{
    public static class BindHelper
    {
        public static IEnumerable<T> Bind<T, TEnum>(Action<T, TEnum> mutator = null) where TEnum : struct where T : class
        {
            var result = new List<T>();
            var enumType = typeof(TEnum);
            var type = typeof(T);

            if (enumType.IsEnum == false)
                throw new ArgumentException("TEnum must be an enumerated type.");

            var typeConstructor = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null);
            if (typeConstructor == null)
                return result;

            var values = Enum.GetValues(enumType);

            foreach (var value in values)
            {
                var fieldInfo = value.GetType().GetField(value.ToString());
                var bindAttributes = (BindPropertyAttribute[])fieldInfo.GetCustomAttributes(typeof(BindPropertyAttribute), false);

                var obj = (T)Activator.CreateInstance(type, true);

                mutator?.Invoke(obj, (TEnum)value);

                foreach (var attribute in bindAttributes)
                    obj.SetProperty(attribute.PropertyName, attribute.PropertyValue);
                result.Add(obj);
            }

            return result;
        }
    }
}
