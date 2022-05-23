using Structr.Navigation;
using System.Collections.Generic;

namespace Structr.Tests.Navigation.TestUtils.Extensions
{
    internal static class NavigationItemEnumerableExtensions
    {
        public static int RecursivelyCountActiveChildren<TNavigationItem>(this IEnumerable<NavigationItem<TNavigationItem>> items)
            where TNavigationItem : NavigationItem<TNavigationItem>, new()
        {
            int activeCount = 0;
            foreach (var item in items)
            {
                if (item.IsActive)
                {
                    activeCount++;
                }
                if (item.HasChildren)
                {
                    activeCount += item.Children.RecursivelyCountActiveChildren();
                }
            }
            return activeCount;
        }
    }
}
