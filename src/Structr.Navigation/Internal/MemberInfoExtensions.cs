using System.Reflection;

namespace Structr.Navigation.Internal
{
    /// <summary>
    /// Extension methods for <see cref="MemberInfo"/>.
    /// </summary>
    internal static class MemberInfoExtensions
    {
        /// <summary>
        /// Determines if the specified member has a setter.
        /// </summary>
        /// <param name="member">The <see cref="MemberInfo"/>.</param>
        /// <returns>Returns <see langword="true"/> if the specified member has a setter, otherwise returns <see langword="false"/>.</returns>
        internal static bool IsPropertyWithSetter(this MemberInfo member)
        {
            var property = member as PropertyInfo;
            return property?.SetMethod != null;
        }
    }
}
