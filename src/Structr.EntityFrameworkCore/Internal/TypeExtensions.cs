using System;

namespace Structr.EntityFrameworkCore.Internal
{
    internal static class TypeExtensions
    {
        public static bool IsAssignableFromGenericType(this Type genericType, Type type)
        {
            if (genericType == null)
            {
                throw new ArgumentNullException(nameof(genericType));
            }
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

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
