using System;

namespace Structr.Domain
{
    /// <summary>
    /// General class for an entity <see cref="TEntity"/>.
    /// </summary>
    /// <typeparam name="TEntity">Type of entity.</typeparam>
    public abstract class Entity<TEntity> : IEquatable<TEntity>
        where TEntity : Entity<TEntity>
    {
        protected int? CachedHashCode;

        protected Entity()
        {
            if ((this as TEntity) == null)
            {
                throw new InvalidOperationException(
                    $"Entity '{GetType()}' specifies '{typeof(TEntity).Name}' as generic argument, it should be its own type");
            }
        }

        /// <summary>
        /// Defines if the entity is transient.
        /// </summary>
        public abstract bool IsTransient();

        public abstract bool Equals(TEntity other);
        protected abstract int GenerateHashCode();

        public override bool Equals(object obj)
        {
            return Equals(obj as TEntity);
        }

        public override int GetHashCode()
        {
            if (CachedHashCode.HasValue)
            {
                return CachedHashCode.Value;
            }

            if (IsTransient())
            {
                return base.GetHashCode();
            }
            else
            {
                unchecked
                {
                    CachedHashCode = GenerateHashCode();
                }
            }

            return CachedHashCode.Value;
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

    /// <summary>
    /// General class for an entity <see cref="TEntity"/> with identifier <see cref="TKey"/>.
    /// </summary>
    /// <typeparam name="TEntity">Type of entity.</typeparam>
    /// <typeparam name="TKey">Type of entity identifier.</typeparam>
    public abstract class Entity<TEntity, TKey> : Entity<TEntity>
        where TEntity : Entity<TEntity, TKey>
    {
        /// <summary>
        /// The entity identifier.
        /// </summary>
        public virtual TKey Id { get; protected set; }

        protected Entity() : base() { }

        /// <summary>
        /// Defines if the entity is transient.
        /// </summary>
        public override bool IsTransient()
        {
            return Equals(Id, default(TKey));
        }

        public override bool Equals(TEntity other)
        {
            if (other == null || !GetType().IsInstanceOfType(other))
            {
                return false;
            }

            if (IsTransient() ^ other.IsTransient())
            {
                return false;
            }

            if (IsTransient() && other.IsTransient())
            {
                return ReferenceEquals(this, other);
            }

            return Equals(Id, other.Id);
        }

        protected override int GenerateHashCode()
        {
            return (GetType().GetHashCode() * 31) ^ Id.GetHashCode();
        }

        public override string ToString()
        {
            return GetType().Name + $" [{nameof(Id)}={Id}]";
        }
    }
}
