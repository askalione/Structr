using Structr.Notices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Structr.Domain
{
    public abstract class Entity<TEntity> : IEquatable<TEntity>
        where TEntity : Entity<TEntity>
    {
        private readonly Queue<INotice> _events = new Queue<INotice>();
        public virtual IReadOnlyCollection<INotice> Events => new ReadOnlyCollection<INotice>(_events.ToList());

        protected Entity()
        {
            if ((this as TEntity) == null)
            {
                throw new InvalidOperationException(
                    $"Entity '{GetType()}' specifies '{typeof(TEntity).Name}' as generic argument, it should be its own type");
            }
        }

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

        public abstract bool IsTransient();

        public abstract bool Equals(TEntity other);

        public override bool Equals(object obj)
        {
            return Equals(obj as TEntity);
        }

        public static bool operator ==(Entity<TEntity> left, Entity<TEntity> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Entity<TEntity> left, Entity<TEntity> right)
        {
            return !(left == right);
        }
    }

    public abstract class Entity<TEntity, TKey> : Entity<TEntity>
        where TEntity : Entity<TEntity, TKey>
        where TKey : IEquatable<TKey>
    {
        public virtual TKey Id { get; protected set; }

        private int? _cachedHashCode;

        protected Entity() : base() { }

        public override bool IsTransient()
        {
            return Equals(Id, default(TKey));
        }

        public override bool Equals(TEntity other)
        {
            if (other == null || !GetType().IsInstanceOfType(other))
                return false;

            if (IsTransient() ^ other.IsTransient())
                return false;

            if (IsTransient() && other.IsTransient())
                return ReferenceEquals(this, other);

            return Equals(Id, other.Id);
        }

        public override int GetHashCode()
        {
            if (_cachedHashCode.HasValue)
                return _cachedHashCode.Value;

            if (IsTransient())
            {
                return base.GetHashCode();
            }
            else
            {
                unchecked
                {
                    _cachedHashCode = (GetType().GetHashCode() * 31) ^ Id.GetHashCode();
                }
            }
            return _cachedHashCode.Value;
        }

        public override string ToString()
        {
            return GetType().Name + " (Id=" + Id.ToString() + ")";
        }
    }
}
