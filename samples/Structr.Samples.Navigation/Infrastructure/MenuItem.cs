using Structr.Navigation;

namespace Structr.Samples.Navigation.Infrastructure
{
    public class MenuItem : NavigationItem<MenuItem>
    {
        public string Action { get; set; }
        public string Controller { get; set; }
        public string Area { get; set; }
        public string Icon { get; set; }
    }
}
