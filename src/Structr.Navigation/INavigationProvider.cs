using Newtonsoft.Json.Linq;

namespace Structr.Navigation
{
    public interface INavigationProvider
    {
        JArray GetNavigation();
    }
}
