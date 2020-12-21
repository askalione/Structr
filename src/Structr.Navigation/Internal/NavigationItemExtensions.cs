using System.Reflection;

namespace Structr.Navigation.Internal
{
    internal static class NavigationItemExtensions
    {
        public static TNavigationItem Clone<TNavigationItem>(this TNavigationItem item)
            where TNavigationItem : NavigationItem<TNavigationItem>, new()
        {
            var clone = new TNavigationItem();
            var type = clone.GetType();
            var props = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var prop in props)
            {
                if (prop.CanWrite == true)
                {
                    prop.SetValue(clone, prop.GetValue(item, null), null);
                }
            }
            return clone;
        }
    }
}
