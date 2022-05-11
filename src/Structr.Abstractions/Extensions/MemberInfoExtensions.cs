using System;
using System.Reflection;

namespace Structr.Abstractions.Extensions
{
    public static class MemberInfoExtensions
    {
        /// <summary>
        /// Gets value of object member.
        /// </summary>
        /// <param name="memberInfo">MemberInfo.</param>
        /// <param name="instance">Object to get value.</param>
        /// <returns>
        /// Value of object memeber as object.
        /// </returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        public static object GetMemberValue(this MemberInfo memberInfo, object instance)
        {
            Ensure.NotNull(memberInfo, nameof(memberInfo));
            Ensure.NotNull(instance, nameof(instance));

            switch (memberInfo.MemberType)
            {
                case MemberTypes.Field:
                    return ((FieldInfo)memberInfo).GetValue(instance);
                case MemberTypes.Property:
                    return ((PropertyInfo)memberInfo).GetValue(instance);
                default:
                    throw new NotSupportedException("Not supported member type");
            }
        }

        /// <summary>
        /// Gets type of object member.
        /// </summary>
        /// <param name="memberInfo">MemberInfo.</param>
        /// <returns>
        /// Type of object member.
        /// </returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        public static Type GetMemberType(this MemberInfo memberInfo)
        {
            Ensure.NotNull(memberInfo, nameof(memberInfo));

            switch (memberInfo.MemberType)
            {
                case MemberTypes.Field:
                    return ((FieldInfo)memberInfo).FieldType;
                case MemberTypes.Property:
                    return ((PropertyInfo)memberInfo).PropertyType;
                case MemberTypes.Event:
                    return ((EventInfo)memberInfo).EventHandlerType;
                case MemberTypes.Method:
                    return ((MethodInfo)memberInfo).ReturnType;
                default:
                    throw new NotSupportedException("Not supported member type");
            }
        }
    }
}
