using System;

namespace Structr.Navigation
{
    /// <summary>
    /// Defines a set of options used for configure services.
    /// </summary>
    /// <typeparam name="TNavigationItem">The class <see cref="NavigationItem{TNavigationItem}"/> implementation.</typeparam>
    public class NavigationOptions<TNavigationItem>
        where TNavigationItem : NavigationItem<TNavigationItem>, new()
    {
        /// <summary>
        /// Determines a type of resources file whether uses for localization.
        /// </summary>
        public Type ResourceType { get; set; }

        /// <summary>
        /// Determines a filter function for navigation items.
        /// </summary>
        public Func<TNavigationItem, bool> ItemFilter { get; set; }

        /// <summary>
        /// Determines an activation function for navigation items.
        /// </summary>
        public Func<TNavigationItem, bool> ItemActivator { get; set; }

        /// <summary>
        /// Determines whether navigation items should be cached.
        /// </summary>
        public bool EnableCaching { get; set; }

        /// <summary>
        /// Initializes an instance of <see cref="NavigationOptions"/> with default values.
        /// </summary>
        public NavigationOptions()
        {
            ItemFilter = item => true;
            ItemActivator = item => false;
            EnableCaching = true;
        }
    }
}
