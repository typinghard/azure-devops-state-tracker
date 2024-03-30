using AzureDevopsTracker.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AzureDevopsTracker.Interfaces.Internals
{
    public interface IRepository<TEntity> : IDisposable where TEntity : Entity
    {
        Task Add(TEntity entity);
        Task Add(IEnumerable<TEntity> entities);
        Task<TEntity> GetById(string id);
        Task<bool> Exist(string id);
        void Update(TEntity entity);
        void Update(IEnumerable<TEntity> entities);
        void Delete(TEntity entity);
        Task SaveChangesAsync();
    }
}