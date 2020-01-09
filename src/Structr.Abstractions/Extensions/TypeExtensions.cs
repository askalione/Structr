using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Structr.Abstractions.Extensions
{
    public static class TypeExtensions
    {
        /// <summary>
        /// Returns a value indicating whether property with a specified name 
        ///     occurs within this type.
        /// </summary>
        /// <param name="type">Type where the property required.</param>
        /// <param name="propertyName">Name of required property.</param>
        /// <returns>
        /// True if type has property with specified name, otherwise, false. 
        /// </returns>
        public static bool HasOwnProperty(this Type type, string propertyName)
        {
            Ensure.NotNull(type, nameof(type));
            Ensure.NotEmpty(propertyName, nameof(propertyName));

            return type
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Any(p => p.Name.Equals(propertyName));
        }

        public static bool HasNestedProperty(this Type type, string propertyName)
        {
            return GetPropertyInfo(type, propertyName) != null;            
        }

        /// <summary>
        /// Returns a value indicating whether source type is nullable enumeration.
        /// </summary>
        /// <param name="type">Type to checking.</param>
        /// <returns>
        /// True if type is nullable enumeration, otherwise, false. 
        /// </returns>
        public static bool IsNullableEnum(this Type type)
        {
            Ensure.NotNull(type, nameof(type));

            Type u = Nullable.GetUnderlyingType(type);
            return (u != null) && u.IsEnum;
        }

        /// <summary>
        /// Return property info by property name of specified type. 
        ///     Include navigation properties e.g. ("Object.Info.DisplayName")
        /// </summary>
        /// <param name="type">Type to get property info;</param>
        /// <param name="propertyName">Name of required property.</param>
        /// <returns>
        /// PropertyInfo object.
        /// </returns>
        public static PropertyInfo GetPropertyInfo(this Type type, string propertyName)
        {
            Ensure.NotNull(type, nameof(type));
            Ensure.NotEmpty(propertyName, nameof(propertyName));

            bool propertyHasDot = propertyName.IndexOf(".") > -1;
            string firstPartBeforeDot;
            string nextParts = "";

            if (!propertyHasDot)
            {
                firstPartBeforeDot = propertyName.ToLower();
            }
            else
            {
                firstPartBeforeDot = propertyName.Substring(0, propertyName.IndexOf(".")).ToLower();
                nextParts = propertyName.Substring(propertyName.IndexOf(".") + 1);
            }

            foreach (var property in type.GetProperties())
                if (property.Name.ToLower() == firstPartBeforeDot)
                    if (!propertyHasDot)
                        return property;
                    else
                        return GetPropertyInfo(property.PropertyType, nextParts);

            return null;
        }

        /// <summary>
        /// Return default value of specified type.
        /// </summary>
        /// <param name="type">Type to get value.</param>
        /// <returns>
        /// Object with default value.
        /// </returns>
        public static object GetDefaultValue(this Type type)
        {
            Ensure.NotNull(type, nameof(type));

            Expression<Func<object>> e = Expression.Lambda<Func<object>>(
                Expression.Convert(
                    Expression.Default(type), typeof(object)
                )
            );

            return e.Compile()();
        }

        public static bool IsAssignableFromGenericType(this Type genericType, Type type)
        {
            Ensure.NotNull(genericType, nameof(genericType));
            Ensure.NotNull(type, nameof(type));

            var interfaceTypes = type.GetInterfaces();

            foreach (var it in interfaceTypes)
            {
                if (it.IsGenericType && it.GetGenericTypeDefinition() == genericType)
                    return true;
            }

            if (type.IsGenericType && type.GetGenericTypeDefinition() == genericType)
                return true;

            Type baseType = type.BaseType;
            if (baseType == null) return false;

            return IsAssignableFromGenericType(genericType, baseType);
        }
    }
}
