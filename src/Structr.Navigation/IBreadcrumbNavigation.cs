using System.Collections.Generic;

namespace Structr.Navigation
{
    public interface IBreadcrumbNavigation<out TBreadcrumb> : IEnumerable<TBreadcrumb> where TBreadcrumb : NavigationItem<TBreadcrumb>
    {
        TBreadcrumb Active { get; }
    }
}
