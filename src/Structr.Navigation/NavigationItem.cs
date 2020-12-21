using System;
using System.Collections.Generic;
using System.Linq;

namespace Structr.Navigation
{
    public abstract class NavigationItem<TNavigationItem> : IEquatable<TNavigationItem>
        where TNavigationItem : NavigationItem<TNavigationItem>, new()
    {
        private TNavigationItem _this => (TNavigationItem)this;
        
        public string Id { get; set; }
        public string Title { get; set; }
        public string ResourceName { get; set; }

        private readonly List<TNavigationItem> _children = new List<TNavigationItem>();
        public IEnumerable<TNavigationItem> Children => _children;

        private readonly List<TNavigationItem> _ancestors = new List<TNavigationItem>();
        public IEnumerable<TNavigationItem> Ancestors => _ancestors;

        private readonly List<TNavigationItem> _descendants = new List<TNavigationItem>();
        public IEnumerable<TNavigationItem> Descendants => _descendants;

        private TNavigationItem _parent = null;
        public TNavigationItem Parent => _parent;

        private bool _isActive = false;
        public bool IsActive => _isActive;

        public bool HasChildren => _children.Any();
        public bool HasActiveChild => _children.Any(x => x.IsActive);
        public bool HasActiveDescendant => _descendants.Any(x => x.IsActive);
        public bool HasActiveAncestor => _ancestors.Any(x => x.IsActive);

        public NavigationItem() { }

        #region Relation

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
