using System;
using System.Collections.Generic;
using System.Linq;

namespace Structr.Navigation
{
    /// <summary>
    /// Base navigation item.
    /// </summary>
    /// <typeparam name="TNavigationItem">The class <see cref="NavigationItem{TNavigationItem}"/> implementation.</typeparam>
    public abstract class NavigationItem<TNavigationItem> : IEquatable<TNavigationItem>
        where TNavigationItem : NavigationItem<TNavigationItem>, new()
    {
        private TNavigationItem _this => (TNavigationItem)this;

        /// <summary>
        /// Navigation item identifier.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Navigation item title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The key of navigation item into resource file (if defined).
        /// </summary>
        public string ResourceName { get; set; }

        /// <summary>
        /// Child navigation elements.
        /// </summary>
        private readonly List<TNavigationItem> _children = new List<TNavigationItem>();

        /// <summary>
        /// Child navigation elements.
        /// </summary>
        public IEnumerable<TNavigationItem> Children => _children;

        /// <summary>
        /// All parent navigation items.
        /// </summary>
        private readonly List<TNavigationItem> _ancestors = new List<TNavigationItem>();

        /// <summary>
        /// All parent navigation items.
        /// </summary>
        public IEnumerable<TNavigationItem> Ancestors => _ancestors;

        /// <summary>
        /// All child navigation items.
        /// </summary>
        private readonly List<TNavigationItem> _descendants = new List<TNavigationItem>();

        /// <summary>
        /// All child navigation items.
        /// </summary>
        public IEnumerable<TNavigationItem> Descendants => _descendants;

        /// <summary>
        /// Parent navigation item.
        /// </summary>
        private TNavigationItem _parent = null;

        /// <summary>
        /// Parent navigation item.
        /// </summary>
        public TNavigationItem Parent => _parent;

        /// <summary>
        /// Status of navigation item.
        /// </summary>
        private bool _isActive = false;

        /// <summary>
        /// Status of navigation item.
        /// </summary>
        public bool IsActive => _isActive;

        /// <summary>
        /// Returns <see langword="true"/> if the navigation item has a child, otherwise returns <see langword="false"/>.
        /// </summary>
        public bool HasChildren => _children.Any();

        /// <summary>
        /// Returns <see langword="true"/> if the navigation item has an active child, otherwise returns <see langword="false"/>.
        /// </summary>
        public bool HasActiveChild => _children.Any(x => x.IsActive);

        /// <summary>
        /// Returns <see langword="true"/> if the navigation item has an active descendant, otherwise returns <see langword="false"/>.
        /// </summary>
        public bool HasActiveDescendant => _descendants.Any(x => x.IsActive);

        /// <summary>
        /// Returns <see langword="true"/> if the navigation item has an active ancestor, otherwise returns <see langword="false"/>.
        /// </summary>
        public bool HasActiveAncestor => _ancestors.Any(x => x.IsActive);

        /// <summary>
        /// Initializes an instance of <see cref="NavigationItem{TNavigationItem}"/>
        /// </summary>
        public NavigationItem() { }

        #region Relation

        /// <summary>
        /// Adds the child to the children list.
        /// </summary>
        /// <param name="child">Child navigation item.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="child"/> is <see langword="null"/>.</exception>
        public void AddChild(TNavigationItem child)
        {
            if (child == null)
            {
                throw new ArgumentNullException(nameof(child));
            }

            _children.Add(child);
            child._parent = _this;

            ConfigureRelation(_this, child, true);
        }

        /// <summary>
        /// Removes the child from the children list.
        /// </summary>
        /// <param name="child">Child navigation item.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="child"/> is <see langword="null"/>.</exception>
        public void RemoveChild(TNavigationItem child)
        {
            if (child == null)
            {
                throw new ArgumentNullException(nameof(child));
            }

            _children.Remove(child);
            child._parent = null;

            ConfigureRelation(_this, child, false);
        }

        private static void ConfigureRelation(TNavigationItem ancestor, TNavigationItem descendant, bool addRelation)
        {
            if (ancestor._parent != null)
            {
                ConfigureRelation(ancestor._parent, descendant, addRelation);
            }

            foreach (var grandDescendant in descendant._children)
            {
                ConfigureRelation(ancestor, grandDescendant, addRelation);
            }

            if (addRelation == true)
            {
                ancestor._descendants.Add(descendant);
                descendant._ancestors.Add(ancestor);
            }
            else
            {
                ancestor._descendants.Remove(descendant);
                descendant._ancestors.Remove(ancestor);
            }
        }

        #endregion

        #region IEquatable

        public bool Equals(TNavigationItem other)
        {
            if (other == null)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(Id) == true || string.IsNullOrWhiteSpace(other.Id) == true)
            {
                return false;
            }

            return Id.Equals(other.Id, StringComparison.OrdinalIgnoreCase);
        }

        #endregion

        internal void SetActive(bool isActive)
            => _isActive = isActive;
    }
}
