using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using IncosafCMS.Core.Data;
using IncosafCMS.Core.DomainModels;
using System.Linq;

namespace IncosafCMS.Core.Services
{
    public interface IService<TEntity> : IService where TEntity : BaseEntity
    {
        List<TEntity> GetAll();
        IQueryable<TEntity> GetAll(params Expression<Func<TEntity, object>>[] includeProperties);
        IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties);
        List<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate);
        PaginatedList<TEntity> GetAll(int pageIndex, int pageSize);
        PaginatedList<TEntity> GetAll(int pageIndex, int pageSize, Expression<Func<TEntity, int>> keySelector, OrderBy orderBy = OrderBy.Ascending);
        PaginatedList<TEntity> GetAll(int pageIndex, int pageSize, Expression<Func<TEntity, int>> keySelector, Expression<Func<TEntity, bool>> predicate, OrderBy orderBy, params Expression<Func<TEntity, object>>[] includeProperties);
        TEntity GetById(int id);
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        Task<List<TEntity>> GetAllAsync();
        Task<PaginatedList<TEntity>> GetAllAsync(int pageIndex, int pageSize);
        Task<PaginatedList<TEntity>> GetAllAsync(int pageIndex, int pageSize, Expression<Func<TEntity, int>> keySelector, OrderBy orderBy = OrderBy.Ascending);
        Task<PaginatedList<TEntity>> GetAllAsync(int pageIndex, int pageSize, Expression<Func<TEntity, int>> keySelector, Expression<Func<TEntity, bool>> predicate, OrderBy orderBy, params Expression<Func<TEntity, object>>[] includeProperties);
        Task<TEntity> GetByIdAsync(int id);
        Task AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
    }
}