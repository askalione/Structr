using Structr.Navigation;

namespace Structr.Samples.Navigation.Infrastructure
{
    public class Breadcrumb : NavigationItem<Breadcrumb>
    {
        public string Action { get; set; }
        public string Controller { get; set; }
        public string Area { get; set; }
    }
}
