using System;

namespace Structr.EntityFramework.Internal
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

            Type[] interfaceTypes = type.GetInterfaces();
            foreach (Type interfaceType in interfaceTypes)
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
