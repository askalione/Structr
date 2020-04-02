using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Structr.Navigation
{
    public abstract class NavigationItem<TNavigationItem> where TNavigationItem : NavigationItem<TNavigationItem>
    {
        private readonly ICollection<TNavigationItem> _ancestors = new HashSet<TNavigationItem>();
        private readonly ICollection<TNavigationItem> _children = new HashSet<TNavigationItem>();
        private readonly ICollection<TNavigationItem> _descendants = new HashSet<TNavigationItem>();

        public string Id { get; protected set; }
        public string Title { get; internal protected set; }

        [JsonIgnore]
        public bool IsActive { get; internal protected set; }

        public IEnumerable<TNavigationItem> Children => _children.ToList();
        internal IEnumerable<TNavigationItem> Ancestors => _ancestors.ToList();
        internal IEnumerable<TNavigationItem> Descendants => _descendants.ToList();

        public bool HasChildren => _children.Any();
        public bool HasActiveChild => _children.Any(x => x.IsActive);
        public bool HasActiveDescendant => _descendants.Any(x => x.IsActive);
        public bool HasActiveAncestor => _ancestors.Any(x => x.IsActive);

        [JsonIgnore]
        protected TNavigationItem Parent { get; private set; }
        protected TNavigationItem This => (TNavigationItem)this;
        private int? _cachedHashCode;

        internal void AddChild(TNavigationItem child)
        {
            if (child == null)
                throw new ArgumentNullException(nameof(child));

            _children.Add(child);
            child.Parent = This;

            ConfigureRelation(This, child);
        }

        private static void ConfigureRelation(TNavigationItem ancestor, TNavigationItem descendant)
        {
            if (ancestor.Parent != null)
                ConfigureRelation(ancestor.Parent, descendant);

            foreach (var grandDescendant in descendant._children)
                ConfigureRelation(ancestor, grandDescendant);

            ancestor._descendants.Add(descendant);
            descendant._ancestors.Add(ancestor);
        }

        public bool Equals(TNavigationItem other)
        {
            if (other == null || !GetType().IsInstanceOfType(other))
                return false;

            return Equals(Id, other.Id);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as TNavigationItem);
        }

        public override int GetHashCode()
        {
            if (_cachedHashCode.HasValue)
                return _cachedHashCode.Value;

            unchecked
            {
                _cachedHashCode = (GetType().GetHashCode() * 31) ^ Id.GetHashCode();
            }

            return _cachedHashCode.Value;
        }
    }
}
