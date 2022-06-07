using System;
using System.Resources;

namespace Structr.Navigation.Internal
{
    /// <summary>
    /// Provides methods for <see cref="ResourceManager"/>.
    /// </summary>
    internal static class ResourceProvider
    {
        /// <summary>
        /// Returns <see cref="ResourceManager"/> for the specified type.
        /// </summary>
        /// <param name="resourceType">The <see cref="Type"/>.</param>
        internal static ResourceManager TryGetResourceManager(Type resourceType)
        {
            if (resourceType == null)
            {
                return null;
            }

            return new ResourceManager(resourceType.FullName, resourceType.Assembly);
        }
    }
}
