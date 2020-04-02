using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Xunit;

namespace Structr.Navigation.Tests
{
    public class NavigationBuilderTests
    {
        [Fact]
        public void Build_SeveralActiveNavigationItems_ResultHasOneActiveNavigationItem()
        {
            var services = new ServiceCollection();
            string path = Path.Combine(new DirectoryInfo(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))
                .Parent.Parent.Parent.FullName, "menu.json");
            services.AddNavigation(config =>
            {
                config.AddJson<MenuItem>(path, options =>
                {
                    options.Activator = (item, serviceProvider) =>
                    {
                        return true;
                    };
                });
            });
            var serviceProvider = services.BuildServiceProvider();
            var navigation = serviceProvider.GetService<INavigation<MenuItem>>();
            var activeMenuItem = navigation.Active;

            int activeCount = RecursivelyCountActiveChildren(navigation);
            var lastMenuItem = GetLastChild(navigation);

            Assert.Equal(1, activeCount);
            Assert.Equal(lastMenuItem.Id, activeMenuItem.Id);
        }

        private static int RecursivelyCountActiveChildren(IEnumerable<MenuItem> items)
        {
            int activeCount = 0;
            foreach (var item in items)
            {
                if (item.IsActive)
                    activeCount++;
                if (item.HasChildren)
                    activeCount += RecursivelyCountActiveChildren(item.Children);
            }
            return activeCount;
        }

        private static MenuItem GetLastChild(IEnumerable<MenuItem> items)
        {
            MenuItem last = items.LastOrDefault();
            if (last != null && last.HasChildren)
            {
                last = GetLastChild(last.Children);
            }
            return last;
        }
    }
}
