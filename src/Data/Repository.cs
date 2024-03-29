using AzureDevopsTracker.Data.Context;
using AzureDevopsTracker.Entities;
using AzureDevopsTracker.Interfaces.Internals;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureDevopsTracker.Data
{
    internal abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        protected readonly AzureDevopsTrackerContext Db;
        protected readonly DbSet<TEntity> DbSet;

        public Repository(AzureDevopsTrackerContext db)
        {
            Db = db;
            DbSet = db.Set<TEntity>();
        }

        public virtual async Task Add(TEntity entity)
        {
            await DbSet.AddAsync(entity);
        }

        public virtual async Task Add(IEnumerable<TEntity> entities)
        {
            await DbSet.AddRangeAsync(entities);
        }

        public virtual void Update(TEntity entity)
        {
            DbSet.Update(entity);
        }

        public virtual void Update(IEnumerable<TEntity> entities)
        {
            DbSet.UpdateRange(entities);
        }

        public virtual void Delete(TEntity entity)
        {
            DbSet.Remove(entity);
        }

        public virtual async Task<TEntity> GetById(string id)
        {
            return await DbSet
                          .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> Exist(string id)
        {
            return await DbSet
                          .AnyAsync(x => x.Id == id);
        }

        public async Task SaveChangesAsync()
        {
            await Db.SaveChangesAsync();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}