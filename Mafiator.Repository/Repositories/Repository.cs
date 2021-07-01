using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Mafiator.Data;
using Mafiator.Entities;
using Mafiator.Repository.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RepoDb;

namespace Mafiator.Repository.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<TEntity> _entities;
        protected readonly IDbConnection connection;

        public Repository(ApplicationDbContext context, IDbConnection connection)
        {
            _context = context;
            this.connection = connection;
            _entities = context.Set<TEntity>();
        }

        public virtual ValueTask<EntityEntry<TEntity>> Add(TEntity entity)
        {
            entity.Id = Ulid.NewUlid();
            return _entities.AddAsync(entity);
        }

        public virtual Task<object> AddFast(TEntity entity)
        {
            entity.Id = Ulid.NewUlid();
            entity.CreatedDate=DateTime.Now;
            entity.ModifiedDate=DateTime.Now;
            return connection.InsertAsync(entity);
        }

        public virtual Task AddRange(IEnumerable<TEntity> entities)
        {
            foreach (var item in entities)
            {
                item.Id = Ulid.NewUlid();
            }

            return _entities.AddRangeAsync(entities);
        }

        public virtual Task<int> AddRangeFast(IEnumerable<TEntity> entities)
        {
            return connection.InsertAllAsync(entities);
        }

        public virtual void Update(TEntity entity)
        {
            _entities.Update(entity);
        }

        public virtual Task<int> UpdateFast(TEntity entity)
        {
            entity.ModifiedDate = DateTime.Now;
            return connection.UpdateAsync(entity);
        }

        public virtual void UpdateRange(IEnumerable<TEntity> entities)
        {
            _entities.UpdateRange(entities);
        }

        public virtual Task<int> UpdateRangeFast(IEnumerable<TEntity> entities)
        {
            return connection.UpdateAllAsync(entities);
        }

        public virtual void Remove(TEntity entity)
        {
            _entities.Remove(entity);
        }

        public virtual Task<int> RemoveFast(TEntity entity)
        {
            return connection.DeleteAsync(entity);
        }

        public virtual void RemoveRange(IEnumerable<TEntity> entities)
        {
            _entities.RemoveRange(entities);
        }

        public virtual Task<int> RemoveRangeFast(IEnumerable<TEntity> entities)
        {
            return connection.DeleteAllAsync(entities);
        }

        public virtual Task<int> Count()
        {
            return _entities.CountAsync();
        }

        public virtual Task<long> CountFast()
        {
            return connection.CountAllAsync<TEntity>();
        }

        public virtual Task<TEntity> Get(Expression<Func<TEntity, bool>> predicate)
        {
            return _entities.Where(predicate).FirstOrDefaultAsync();
        }

        public virtual Task<IEnumerable<TEntity>> GetFast(Expression<Func<TEntity, bool>> predicate)
        {
            return connection.QueryAsync(predicate);
        }

        public Task<List<TEntity>> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return _entities.Where(predicate).ToListAsync();
        }

        public virtual Task<TEntity> Get(Ulid id)
        {
            return _entities.FirstOrDefaultAsync(e => e.Id == id);
        }

        public ValueTask<TEntity> Find(string id)
        {
            return _entities.FindAsync(id);
        }

        public virtual Task<List<TEntity>> GetAll()
        {
            return _entities.AsNoTracking().OrderByDescending(o => o.Id).ToListAsync();
        }

        public virtual Task<IEnumerable<TEntity>> GetAllFast()
        {
            return connection.QueryAllAsync<TEntity>();
        }

        public Task<List<TEntity>> GetPage(int skip, int offset)
        {
            return _entities.AsNoTracking().Skip(skip).Take(offset).OrderByDescending(o => o.Id).ToListAsync();
        }

        public Task<List<TEntity>> GetPage(Expression<Func<TEntity, bool>> expression, int skip, int offset)
        {
            return _entities.AsNoTracking().Where(expression).Skip(skip).Take(offset).OrderByDescending(o => o.Id)
                .ToListAsync();
        }

        public virtual Task<bool> Exists(TEntity entity)
        {
            return connection.ExistsAsync<TEntity>(entity);
        }
        public virtual Task<bool> Exists(Expression<Func<TEntity, bool>> expression)
        {
            return connection.ExistsAsync(expression);
        }
    }
}