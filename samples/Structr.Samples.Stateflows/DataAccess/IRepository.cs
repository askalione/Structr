using Structr.Samples.Stateflows.Domain;
using System;

namespace Structr.Samples.Stateflows.DataAccess
{
    public interface IRepository<TEntity> where TEntity : Entity
    {
        void Add(TEntity entity);
        TEntity Get(Guid id);
    }
}
