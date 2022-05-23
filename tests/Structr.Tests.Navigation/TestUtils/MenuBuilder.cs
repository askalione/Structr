using Structr.Navigation.Internal;
using System.Collections.Generic;

namespace Structr.Tests.Navigation.TestUtils
{
    internal static class MenuBuilder
    {
        public static IEnumerable<InternalNavigationItem> Build()
        {
            var parent1 = new InternalNavigationItem { Id = "Parent_1", Title = "Parent 1" };
            var parent2 = new InternalNavigationItem { Id = "Parent_2", Title = "Parent 2" };
            var parent3 = new InternalNavigationItem { Id = "Parent_3", Title = "Parent 3" };

            var child11 = new InternalNavigationItem { Id = "Child_1_1", Title = "Child 1 1" };
            var child12 = new InternalNavigationItem { Id = "Child_1_2", Title = "Child 1 2" };
            var child21 = new InternalNavigationItem { Id = "Child_2_1", Title = "Child 2 1" };

            child11.AddChild(new InternalNavigationItem { Id = "Child_1_1_1", Title = "Child 1 1 1" });

            parent1.AddChild(child11);
            parent1.AddChild(child12);

            parent2.AddChild(child21);

            var result = new List<InternalNavigationItem> { parent1, parent2, parent3 };
            return result;
        }
    }
}
