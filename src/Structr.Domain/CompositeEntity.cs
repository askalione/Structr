using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Structr.Domain
{
    /// <summary>
    /// Base class for an entity <typeparamref name="TEntity"/> with composite identifier.
    /// </summary>
    /// <typeparam name="TEntity">Type of entity.</typeparam>
    public abstract class CompositeEntity<TEntity> : Entity<TEntity>
       where TEntity : CompositeEntity<TEntity>
    {
        protected CompositeEntity() : base() { }

        protected abstract object GetCompositeId();

        public override bool Equals(TEntity other)
        {
            if (other == null)
            {
                return false;
            }

            object compositeId = GetCompositeId();
            object otherCompositeId = other.GetCompositeId();

            if (compositeId == null || otherCompositeId == null)
            {
                return false;
            }

            Type compositeIdType = compositeId.GetType();

            if (compositeIdType != otherCompositeId.GetType())
            {
                return false;
            }

            return compositeIdType.GetProperties()
                .All(p => Equals(p.GetValue(compositeId, null), p.GetValue(otherCompositeId, null)));
        }

        protected override int GenerateHashCode()
        {
            object compositeId = GetCompositeId();

            if (compositeId == null)
            {
                return 0;
            }

            int hash = 37;

            Type compositeIdType = compositeId.GetType();

            foreach (PropertyInfo property in compositeIdType.GetProperties())
            {
                hash = (hash * 23) + (property.GetValue(compositeId, null)?.GetHashCode() ?? 0);
            }

            return hash;
        }

        public override bool IsTransient()
            => false;

        public override string ToString()
        {
            object compositeId = GetCompositeId();
            string typeName = GetType().Name;

            if (compositeId == null)
            {
                return typeName;
            }
            else
            {
                IEnumerable<string> ids = compositeId.GetType().GetProperties()
                    .Select(p => $"[{p.Name}={p.GetValue(compositeId, null)}]");
                string result = typeName + " " + string.Join("", ids);
                return result;
            }
        }
    }
}
