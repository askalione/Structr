using Structr.Navigation;
using System.Collections.Generic;

namespace Structr.Samples.Navigation.Infrastructure
{
    public class Breadcrumb : NavigationItem<Breadcrumb>
    {
        public string Icon { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public string Area { get; set; }
        public Dictionary<string, string> RouteValues { get; set; }
        public List<string> PreservedRouteValues { get; set; }
    }
}
