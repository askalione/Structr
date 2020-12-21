using System;
using System.Resources;

namespace Structr.Navigation.Internal
{
    internal static class ResourceProvider
    {
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
