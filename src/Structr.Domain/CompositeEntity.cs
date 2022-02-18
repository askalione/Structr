using System.Linq;

namespace Structr.Domain
{
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

            var compositeId = GetCompositeId();
            var otherCompositeId = other.GetCompositeId();

            if (compositeId == null || otherCompositeId == null)
            {
                return false;
            }

            var compositeIdType = compositeId.GetType();

            if (compositeIdType != otherCompositeId.GetType())
            {
                return false;
            }

            return compositeIdType.GetProperties()
                .All(p => object.Equals(p.GetValue(compositeId, null), p.GetValue(otherCompositeId, null)));
        }

        protected override int GenerateHashCode()
        {
            var compositeId = GetCompositeId();

            if (compositeId == null)
            {
                return 0;
            }

            int hash = 37;

            var compositeIdType = compositeId.GetType();

            foreach (var property in compositeIdType.GetProperties())
            {
                hash = hash * 23 + property.GetValue(compositeId, null)?.GetHashCode() ?? 0;
            }

            return hash;
        }

        public override bool IsTransient()
            => false;

        public override string ToString()
        {
            var compositeId = GetCompositeId();
            var typeName = GetType().Name;
            return compositeId == null
                ? typeName
                : typeName + " " + string.Join("", compositeId.GetType().GetProperties()
                    .Select(p => $"[{p.Name}={p.GetValue(compositeId, null)}]"));
        }
    }
}
