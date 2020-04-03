using System.Collections.Generic;

namespace Structr.Navigation
{
    public interface INavigation<out TMenuItem> : IEnumerable<TMenuItem> where TMenuItem : NavigationItem<TMenuItem>
    {
        TMenuItem Active { get; }
    }
}
