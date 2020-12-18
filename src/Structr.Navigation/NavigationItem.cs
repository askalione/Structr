using System;
using System.Collections.Generic;

namespace Structr.Navigation
{
    public abstract class NavigationItem<TNavigationItem> : IEquatable<TNavigationItem> where TNavigationItem : NavigationItem<TNavigationItem>
    {
        private TNavigationItem _this => (TNavigationItem)this;
        private readonly ICollection<TNavigationItem> _ancestors = new HashSet<TNavigationItem>();
        private readonly ICollection<TNavigationItem> _children = new HashSet<TNavigationItem>();
        private readonly ICollection<TNavigationItem> _descendants = new HashSet<TNavigationItem>();

        public string Id { get; protected set; }
        public string Title { get; protected set; }
        public string ResourceName { get; protected set; }

        public IEnumerable<TNavigationItem> Children => _children;
        public IEnumerable<TNavigationItem> Ancestors => _ancestors;
        public IEnumerable<TNavigationItem> Descendants => _descendants;

        public TNavigationItem Parent { get; private set; }

        protected NavigationItem() { }

        public NavigationItem(string id, string title, string resourceName) : this()
        {
            if (string.IsNullOrWhiteSpace(id) == true)
            {
                throw new ArgumentNullException(nameof(id));
            }
            if (string.IsNullOrWhiteSpace(title) == true)
            {
                throw new ArgumentNullException(nameof(title));
            }
            if (string.IsNullOrWhiteSpace(resourceName) == true)
            {
                throw new ArgumentNullException(nameof(resourceName));
            }

            Id = id;
            Title = title;
            ResourceName = resourceName;
        }

        public void AddChild(TNavigationItem child)
        {
            if (child == null)
            {
                throw new ArgumentNullException(nameof(child));
            }

            _children.Add(child);
            child.Parent = _this;

            ConfigureRelation(_this, child);
        }

        private static void ConfigureRelation(TNavigationItem ancestor, TNavigationItem descendant)
        {
            if (ancestor.Parent != null)
            {
                ConfigureRelation(ancestor.Parent, descendant);
            }

            foreach (var grandDescendant in descendant._children)
            {
                ConfigureRelation(ancestor, grandDescendant);
            }

            ancestor._descendants.Add(descendant);
            descendant._ancestors.Add(ancestor);
        }

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
    }
}
