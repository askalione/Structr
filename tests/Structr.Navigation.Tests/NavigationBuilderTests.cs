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
            services.AddNavigation()
                .AddJson<MenuItem>(path, (serviceProvider, options) =>
                {
                    options.ItemActivator = item =>
                    {
                        return true;
                    };
                });
            var serviceProvider = services.BuildServiceProvider();
            var navigation = serviceProvider.GetService<INavigation<MenuItem>>();
            var activeMenuItem = navigation.Active;

            int activeCount = RecursivelyCountActiveChildren(navigation);
            var firstMenuItem = navigation.FirstOrDefault();

            Assert.Equal(1, activeCount);
            Assert.Equal(firstMenuItem.Id, activeMenuItem.Id);
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
    }
}
