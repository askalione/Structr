using System.Collections.Generic;

namespace Structr.Navigation
{
    public interface IBreadcrumbNavigation<TBreadcrumb> : IEnumerable<TBreadcrumb> where TBreadcrumb : NavigationItem<TBreadcrumb>
    {
        TBreadcrumb Active { get; }
    }
}
