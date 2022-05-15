using Structr.Abstractions.Attributes;
using Structr.Abstractions.Extensions;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Structr.Abstractions.Helpers
{
    /// <summary>
    /// Provides functionality for generating instaces of objects on base of some Enum, while
    /// binding data containing in enum attributes to coresponding properties of specified objects.
    /// </summary>
    public static class BindHelper
    {
        /// <summary>
        /// Creates and populates array of items with type <typeparamref name="T"/> using provided enum structure of type <typeparamref name="TEnum"/>.
        /// To fill properties of items <see cref="BindPropertyAttribute"/>s attached to enum elements are used.
        /// </summary>
        /// <typeparam name="T">Type of items to add to resulting list</typeparam>
        /// <typeparam name="TEnum">Type of enumeration to use as data source</typeparam>
        /// <param name="mutator"></param>
        /// <returns>Array of elements populated from provided enumeration</returns>
        public static IEnumerable<T> Bind<T, TEnum>(Action<T, TEnum> mutator = null)
            where TEnum : Enum
            where T : class
        {
            var result = new List<T>();
            var enumType = typeof(TEnum);
            var type = typeof(T);

            var typeConstructor = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null);
            if (typeConstructor == null)
            {
                return result;
            }

            var values = Enum.GetValues(enumType);

            foreach (var value in values)
            {
                var fieldInfo = value.GetType().GetField(value.ToString());
                var bindAttributes = (BindPropertyAttribute[])fieldInfo.GetCustomAttributes(typeof(BindPropertyAttribute), false);

                var obj = (T)Activator.CreateInstance(type, true);

                mutator?.Invoke(obj, (TEnum)value);

                foreach (var attribute in bindAttributes)
                {
                    obj.SetProperty(attribute.PropertyName, attribute.PropertyValue);
                }
                result.Add(obj);
            }

            return result;
        }
    }
}
