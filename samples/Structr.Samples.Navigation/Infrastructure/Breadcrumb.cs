using Structr.Navigation;

namespace Structr.Samples.Navigation.Infrastructure
{
    public class Breadcrumb : NavigationItem<Breadcrumb>
    {
        public string Action { get; private set; }
        public string Controller { get; private set; }
        public string Area { get; private set; }
    }
}
