using System.Collections.Generic;

namespace Structr.Navigation
{
    public interface INavigation<TMenuItem> : IEnumerable<TMenuItem> where TMenuItem : NavigationItem<TMenuItem>
    {
        TMenuItem Active { get; }
    }
}
