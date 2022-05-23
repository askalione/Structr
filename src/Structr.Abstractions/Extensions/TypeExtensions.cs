using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Structr.Abstractions.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="Type"/>.
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Gets a value indicating whether public non-static property with a specified name occurs directly within this type.
        /// </summary>
        /// <param name="type">Type to check.</param>
        /// <param name="propertyName">Name of required property.</param>
        /// <returns>
        /// <see langword="true"/> if type has property with specified name, otherwise <see langword="false"/>. 
        /// </returns>
        public static bool HasOwnProperty(this Type type, string propertyName)
        {
            Ensure.NotNull(type, nameof(type));
            Ensure.NotEmpty(propertyName, nameof(propertyName));

            return type
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Any(p => p.Name.Equals(propertyName));
        }

        /// <summary>
        /// Gets a value indicating whether public non-static property with a specified name occurs within this type or it's "nested" types.
        /// </summary>
        /// <param name="type">Type to check.</param>
        /// <param name="propertyName">Name of required property.</param>
        /// <returns>
        /// <see langword="true"/> if type has property with specified name, otherwise <see langword="false"/>. 
        /// </returns>
        public static bool HasNestedProperty(this Type type, string propertyName)
        {
            return GetPropertyInfo(type, propertyName) != null;
        }

        /// <summary>
        /// Gets a value indicating whether source type is nullable enumeration.
        /// </summary>
        /// <param name="type">Type to checking.</param>
        /// <returns>
        /// <see langword="true"/> if type is nullable enumeration, otherwise <see langword="false"/>. 
        /// </returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool IsNullableEnum(this Type type)
        {
            Ensure.NotNull(type, nameof(type));

            Type underlyingType = Nullable.GetUnderlyingType(type);
            return (underlyingType != null) && underlyingType.IsEnum;
        }

        /// <summary>
        /// Returns <see cref="PropertyInfo"/> by full property name or <see langword="null"/> if no property
        /// was found. (for example "Object.SomeNestedObject.DisplayName").
        /// </summary>
        /// <param name="type">Type to get property info.</param>
        /// <param name="propertyName">Full name of required property with dots if needed.</param>
        /// <returns>
        /// <see cref="PropertyInfo"/> object.
        /// </returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static PropertyInfo GetPropertyInfo(this Type type, string propertyName)
        {
            Ensure.NotNull(type, nameof(type));
            Ensure.NotEmpty(propertyName, nameof(propertyName));

            bool propertyHasDot = propertyName.IndexOf(".") > -1;
            string firstPartBeforeDot;
            string nextParts = "";

            if (propertyHasDot == false)
            {
                firstPartBeforeDot = propertyName.ToLower();
            }
            else
            {
                firstPartBeforeDot = propertyName.Substring(0, propertyName.IndexOf(".")).ToLower();
                nextParts = propertyName.Substring(propertyName.IndexOf(".") + 1);
            }

            foreach (var property in type.GetProperties())
            {
                if (property.Name.ToLower() == firstPartBeforeDot)
                {
                    if (propertyHasDot == false)
                    {
                        return property;
                    }
                    else
                    {
                        return GetPropertyInfo(property.PropertyType, nextParts);
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Return default value of specified type.
        /// </summary>
        /// <param name="type">Type to get value.</param>
        /// <returns>
        /// Object with default value.
        /// </returns>
        /// <remarks>Could be replaced with <see langword="default"/> keyword</remarks>
        public static object GetDefaultValue(this Type type)
        {
            Ensure.NotNull(type, nameof(type));

            Expression<Func<object>> expression = Expression.Lambda<Func<object>>(
                Expression.Convert(
                    Expression.Default(type), typeof(object)
                )
            );

            return expression.Compile()();
        }

        /// <summary>
        /// Determines whether an instance of a specified type can be assigned to an instance
        /// of the current type taking into account generic nature of specified type.
        /// </summary>
        /// <param name="genericType"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool IsAssignableFromGenericType(this Type genericType, Type type)
        {
            Ensure.NotNull(genericType, nameof(genericType));
            Ensure.NotNull(type, nameof(type));

            var interfaceTypes = type.GetInterfaces();

            foreach (var interfaceType in interfaceTypes)
            {
                if (interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == genericType)
                {
                    return true;
                }
            }

            if (type.IsGenericType && type.GetGenericTypeDefinition() == genericType)
            {
                return true;
            }

            Type baseType = type.BaseType;
            if (baseType == null)
            {
                return false;
            }

            return IsAssignableFromGenericType(genericType, baseType);
        }
    }
}
