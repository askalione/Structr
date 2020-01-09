using Structr.Samples.Stateflows.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Structr.Samples.Stateflows.DataAccess
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        private readonly List<TEntity> _storage = new List<TEntity>();

        public void Add(TEntity entity)
        {
            if (!_storage.Any(x => x.Id == entity.Id))
                _storage.Add(entity);
        }

        public TEntity Get(Guid id)
        {
            return _storage.FirstOrDefault(x => x.Id == id);
        }
    }
}
