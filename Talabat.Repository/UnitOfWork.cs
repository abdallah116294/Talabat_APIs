using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreContext _storeContext;
        private readonly Dictionary<Type, object> _repositories;//Key Value Pair Dictionary to hold repositories 
        private readonly Hashtable _repositoryCache; // Cache for repositories
        public UnitOfWork(StoreContext storeContext )

        {
            _storeContext = storeContext;
            _repositories = new Dictionary<Type, object>();
            _repositoryCache = new Hashtable();
        }
        public Task<int> CompleteAsync()
        {
            return _storeContext.SaveChangesAsync();
        }

        public async ValueTask DisposeAsync()
        {
            await _storeContext.DisposeAsync();
        }

        public IGenericRepositort<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            //use the Hashtable to cache repositories
            // var type = typeof(TEntity).Name;
            // if (!_repositoryCache.ContainsKey(type))
            // {
            //     var repository=new GenericRepository<TEntity>(_storeContext);
            //     _repositoryCache.Add(type, repository);
            //     // return (IGenericRepositort<TEntity>)_repositoryCache[type];
            // }
            //return (IGenericRepositort<TEntity>)_repositoryCache[type];

            //using Dictionary to hold repositories
            var type = typeof(TEntity);
            if (!_repositories.ContainsKey(type))
            {
                var repositorInstance = new GenericRepository<TEntity>(_storeContext);
                _repositories.Add(type, repositorInstance);
            }
            return (IGenericRepositort<TEntity>)_repositories[type];
            //return _repositories.TryGetValue(typeof(TEntity), out var repository)
            // ? (IGenericRepositort<TEntity>)repository
            // : throw new KeyNotFoundException($"Repository for type {typeof(TEntity).Name} not found.");
        }
    }
}
