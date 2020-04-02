using System.Collections.Generic;

namespace Structr.Navigation
{
    public interface IMenuNavigation<TMenuItem> : IEnumerable<TMenuItem> where TMenuItem : NavigationItem<TMenuItem>
    {
        TMenuItem Active { get; }
    }
}
