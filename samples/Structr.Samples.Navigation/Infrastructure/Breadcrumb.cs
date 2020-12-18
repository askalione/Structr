using Structr.Navigation;
using System.Collections.Generic;

namespace Structr.Samples.Navigation.Infrastructure
{
    public class Breadcrumb : NavigationItem<Breadcrumb>
    {
        public string Icon { get; private set; }
        public string Action { get; private set; }
        public string Controller { get; private set; }
        public string Area { get; private set; }
        public Dictionary<string, string> RouteValues { get; private set; }
        public List<string> PreservedRouteValues { get; private set; }
    }
}
