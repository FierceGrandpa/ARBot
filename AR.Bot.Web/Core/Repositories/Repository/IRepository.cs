using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace AR.Bot.Repositories
{
    // TODO: Refactor
    public interface IRepository<T> where T : class
    {
        #region Command

        void Add(T item);
        void AddRange(IEnumerable<T> items);

        void Delete(object key);
        void Delete(Expression<Func<T, bool>> where);
        void DeleteRange(IEnumerable<T> items);

        void Update(object key, T item);
        void UpdatePartial(object key, object item);
        void UpdateRange(IEnumerable<T> items);

        Task AddAsync(T item);
        Task AddRangeAsync(IEnumerable<T> items);

        Task DeleteAsync(object key);
        Task DeleteAsync(Expression<Func<T, bool>> where);
        Task DeleteRangeAsync(IEnumerable<T> items);


        Task UpdateAsync(object key, T item);
        Task UpdatePartialAsync(object key, object item);
        Task UpdateRangeAsync(IEnumerable<T> items);

        #endregion

        #region Query

        IQueryable<T> Queryable { get; }
        IQueryable<T> QueryableNoTracking { get; }

        bool Any();
        bool Any(Expression<Func<T, bool>> where);

        long Count();
        long Count(Expression<Func<T, bool>> where);

        T Get(object key);
        IReadOnlyList<T> GetAll();

        IEnumerable<T> GetWithInclude(Func<T, bool> predicate, params Expression<Func<T, object>>[] includeProperties);

        IEnumerable<T> List();

        Task<bool> AnyAsync();
        Task<bool> AnyAsync(Expression<Func<T, bool>> where);

        Task<long> CountAsync();
        Task<long> CountAsync(Expression<Func<T, bool>> where);

        Task<T> GetAsync(object key);
        Task<IReadOnlyList<T>> GetAllAsync();

        Task<IEnumerable<T>> ListAsync();

        #endregion
    }
}
