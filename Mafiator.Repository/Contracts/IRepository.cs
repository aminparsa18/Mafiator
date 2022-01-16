using Mafiator.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mafiator.Repository.Contracts
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        ValueTask<EntityEntry<TEntity>> Add(TEntity entity);
        Task<object> AddFast(TEntity entity);
        Task AddRange(IEnumerable<TEntity> entities);
        Task<int> AddRangeFast(IEnumerable<TEntity> entities);
        void Update(TEntity entity);
        Task<int> UpdateFast(TEntity entity);
        void UpdateRange(IEnumerable<TEntity> entities);
        Task<int> UpdateRangeFast(IEnumerable<TEntity> entities);
        void Remove(TEntity entity);
        Task<int> RemoveFast(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);
        Task<int> RemoveRangeFast(IEnumerable<TEntity> entities);
        Task<int> Count();
        Task<long> CountFast();
        ValueTask<TEntity> Find(string id);
        Task<List<TEntity>> Find(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> Get(Guid id);
        Task<TEntity> Get(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> GetFast(Expression<Func<TEntity, bool>> predicate);
        Task<List<TEntity>> GetAll();
        Task<IEnumerable<TEntity>> GetAllFast();
        Task<bool> Exists(TEntity entity);
        Task<bool> Exists(Expression<Func<TEntity, bool>> expression);
        Task<List<TEntity>> GetPage(int skip, int offset);
        Task<List<TEntity>> GetPage(Expression<Func<TEntity, bool>> expression, int skip, int offset);
    }
}
