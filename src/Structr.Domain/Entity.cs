using Structr.Notices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Structr.Domain
{
    public abstract class Entity<TKey> : IEquatable<Entity<TKey>>
    {
        public virtual TKey Id { get; protected set; }

        private int? _cachedHashCode;
        private readonly Queue<INotice> _events = new Queue<INotice>();
        public virtual IReadOnlyCollection<INotice> Events => new ReadOnlyCollection<INotice>(_events.ToList());

        protected virtual void ApplyEvent(INotice @event)
        {
            if (@event == null)
                throw new ArgumentNullException(nameof(@event));

            _events.Enqueue(@event);
        }

        public virtual void ClearEvents()
        {
            _events.Clear();
        }

        public virtual bool IsTransient()
        {
            return Equals(Id, default(TKey));
        }

        public virtual bool Equals(Entity<TKey> other)
        {
            if (other == null || !GetType().IsInstanceOfType(other))
                return false;

            if (IsTransient() ^ other.IsTransient())
                return false;

            if (IsTransient() && other.IsTransient())
                return ReferenceEquals(this, other);

            return Equals(Id, other.Id);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Entity<TKey>);
        }

        public static bool operator ==(Entity<TKey> left, Entity<TKey> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Entity<TKey> left, Entity<TKey> right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            if (IsTransient())
                return base.GetHashCode();

            if (_cachedHashCode.HasValue)
                return _cachedHashCode.Value;

            _cachedHashCode = GetType().GetHashCode() ^ Id.GetHashCode();
            return _cachedHashCode.Value;
        }

        public override string ToString()
        {
            return GetType().Name + " (Id=" + Id.ToString() + ")";
        }
    }
}
