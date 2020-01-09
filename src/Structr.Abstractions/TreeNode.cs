using System.Collections.Generic;

namespace Structr.Abstractions
{
    public abstract class TreeNode<T> where T : TreeNode<T>
    {
        private readonly ICollection<T> _ancestors = new HashSet<T>();
        private readonly ICollection<T> _children = new HashSet<T>();
        private readonly ICollection<T> _descendants = new HashSet<T>();

        public virtual T Parent { get; private set; }
        public virtual IEnumerable<T> Children => _children;
        public virtual IEnumerable<T> Ancestors => _ancestors;
        public virtual IEnumerable<T> Descendants => _descendants;

        protected T This => (T)this;

        public bool HasChildren => _children.Count > 0;
        public bool HasParent => Parent != null;

        public virtual void AddChild(T child)
        {
            Ensure.NotNull(child, nameof(child));

            _children.Add(child);
            child.Parent = This;

            SetAncestorDescendantRelation(This, child);
        }

        public virtual void ClearParent()
        {
            if (Parent == null)
                return;

            UnsetAncestorDescendantRelation(Parent, This);
            Parent._children.Remove(This);
            Parent = null;
        }

        private static void UnsetAncestorDescendantRelation(T ancestor, T descendant)
        {
            ChangeAncestorDescendantRelation(ancestor, descendant, false);
        }

        private static void SetAncestorDescendantRelation(T ancestor, T descendant)
        {
            ChangeAncestorDescendantRelation(ancestor, descendant, true);
        }

        private static void ChangeAncestorDescendantRelation(T ancestor, T descendant, bool addRelation)
        {
            if (ancestor.Parent != null)
                ChangeAncestorDescendantRelation(ancestor.Parent, descendant, addRelation);

            foreach (T grandDescendant in descendant._children)
                ChangeAncestorDescendantRelation(ancestor, grandDescendant, addRelation);

            if (addRelation)
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
    }
}
