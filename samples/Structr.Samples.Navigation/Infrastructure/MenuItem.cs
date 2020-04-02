using Structr.Navigation;

namespace Structr.Samples.Navigation.Infrastructure
{
    public class MenuItem : NavigationItem<MenuItem>
    {
        public string Action { get; private set; }
        public string Controller { get; private set; }
        public string Area { get; private set; }
        public string Icon { get; private set; }
    }
}
