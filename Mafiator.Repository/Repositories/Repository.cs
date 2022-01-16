using Mafiator.Data;
using Mafiator.Entities;
using Mafiator.Repository.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RepoDb;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mafiator.Repository.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly ApplicationDbContext Context;
        protected readonly DbSet<TEntity> Entities;
        protected readonly IDbConnection Connection;

        public Repository(ApplicationDbContext context, IDbConnection connection)
        {
            Context = context;
            this.Connection = connection;
            Entities = context.Set<TEntity>();
        }

        public virtual ValueTask<EntityEntry<TEntity>> Add(TEntity entity)
        {
            entity.Id = Guid.NewGuid();
            return Entities.AddAsync(entity);
        }

        public virtual Task<object> AddFast(TEntity entity)
        {
            entity.Id = Guid.NewGuid();
            entity.CreatedDate=DateTime.Now;
            entity.ModifiedDate=DateTime.Now;
            return Connection.InsertAsync(ClassMappedNameCache.Get<TEntity>(), entity);
        }

        public virtual Task AddRange(IEnumerable<TEntity> entities)
        {
            foreach (var item in entities)
            {
                item.Id = Guid.NewGuid();
            }

            return Entities.AddRangeAsync(entities);
        }

        public virtual Task<int> AddRangeFast(IEnumerable<TEntity> entities)
        {
            return Connection.InsertAllAsync(entities);
        }

        public virtual void Update(TEntity entity)
        {
            Entities.Update(entity);
        }

        public virtual Task<int> UpdateFast(TEntity entity)
        {
            entity.ModifiedDate = DateTime.Now;
            return Connection.UpdateAsync(entity);
        }

        public virtual void UpdateRange(IEnumerable<TEntity> entities)
        {
            Entities.UpdateRange(entities);
        }

        public virtual Task<int> UpdateRangeFast(IEnumerable<TEntity> entities)
        {
            return Connection.UpdateAllAsync(entities);
        }

        public virtual void Remove(TEntity entity)
        {
            Entities.Remove(entity);
        }

        public virtual Task<int> RemoveFast(TEntity entity)
        {
            return Connection.DeleteAsync(entity);
        }

        public virtual void RemoveRange(IEnumerable<TEntity> entities)
        {
            Entities.RemoveRange(entities);
        }

        public virtual Task<int> RemoveRangeFast(IEnumerable<TEntity> entities)
        {
            return Connection.DeleteAllAsync(entities);
        }

        public virtual Task<int> Count()
        {
            return Entities.CountAsync();
        }

        public virtual Task<long> CountFast()
        {
            return Connection.CountAllAsync<TEntity>();
        }

        public virtual Task<TEntity> Get(Expression<Func<TEntity, bool>> predicate)
        {
            return Entities.Where(predicate).FirstOrDefaultAsync();
        }

        public virtual Task<IEnumerable<TEntity>> GetFast(Expression<Func<TEntity, bool>> predicate)
        {
            return Connection.QueryAsync(predicate);
        }

        public Task<List<TEntity>> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return Entities.Where(predicate).ToListAsync();
        }

        public virtual Task<TEntity> Get(Guid id)
        {
            return Entities.FirstOrDefaultAsync(e => e.Id == id);
        }

        public ValueTask<TEntity> Find(string id)
        {
            return Entities.FindAsync(id);
        }

        public virtual Task<List<TEntity>> GetAll()
        {
            return Entities.AsNoTracking().OrderByDescending(o => o.Id).ToListAsync();
        }

        public virtual Task<IEnumerable<TEntity>> GetAllFast()
        {
            return Connection.QueryAllAsync<TEntity>();
        }

        public Task<List<TEntity>> GetPage(int skip, int offset)
        {
            return Entities.AsNoTracking().Skip(skip).Take(offset).OrderByDescending(o => o.Id).ToListAsync();
        }

        public Task<List<TEntity>> GetPage(Expression<Func<TEntity, bool>> expression, int skip, int offset)
        {
            return Entities.AsNoTracking().Where(expression).Skip(skip).Take(offset).OrderByDescending(o => o.Id)
                .ToListAsync();
        }

        public virtual Task<bool> Exists(TEntity entity)
        {
            return Connection.ExistsAsync<TEntity>(entity);
        }
        public virtual Task<bool> Exists(Expression<Func<TEntity, bool>> expression)
        {
            return Connection.ExistsAsync(expression);
        }
    }
}