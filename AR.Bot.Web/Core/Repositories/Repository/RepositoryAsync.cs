

// ReSharper disable once CheckNamespace

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AR.Bot.Core.Data;
using Microsoft.EntityFrameworkCore;

// ReSharper disable once CheckNamespace
namespace AR.Bot.Repositories
{
    public class RepositoryAsync<T> : IRepository<T> where T : class
    {
        private readonly DataContext _context;
        
        public RepositoryAsync(DataContext context) => _context = context;
        
        #region Command

        private DbSet<T> Entities => _context.CommandSet<T>();

        public void Add(T item) => Entities.Add(item);
        public void AddRange(IEnumerable<T> items) => Entities.AddRange(items);

        public Task AddAsync(T item) => Entities.AddAsync(item).AsTask();
        public Task AddRangeAsync(IEnumerable<T> items) => Entities.AddRangeAsync(items);

        public void Delete(object key)
        {
            var item = Entities.Find(key);

            if (item is null) return;

            Entities.Remove(item);
        }
        public void DeleteRange(IEnumerable<T> items)
        {
            var enumerable = items as T[] ?? items.ToArray();

            if (!enumerable.Any()) return;

            Entities.RemoveRange(enumerable);
        }
        public void Delete(Expression<Func<T, bool>> where) => DeleteRange(Entities.Where(where));

        public Task DeleteAsync(object key) => Task.Run(() => Delete(key));
        public Task DeleteRangeAsync(IEnumerable<T> items) => Task.Run(() => Delete(items));
        public Task DeleteAsync(Expression<Func<T, bool>> where) => Task.Run(() => Delete(where));

        public void Update(object key, T item) => Entities.Update(item);
        public void UpdateRange(IEnumerable<T> items) => Entities.UpdateRange(items);
        public void UpdatePartial(object key, object item)
        {
            var entity = Entities.Find(key);

            if (entity is null) return;

            _context.Entry(entity).CurrentValues.SetValues(item);
        }

        public Task UpdateAsync(object key, T item) => Task.Run(() => Update(key, item));
        public Task UpdateRangeAsync(IEnumerable<T> items) => Task.Run(() => UpdateRange(items));
        public Task UpdatePartialAsync(object key, object item) => Task.Run(() => UpdatePartial(key, item));

        #endregion

        #region Query

        public IQueryable<T> Queryable => _context.QuerySet<T>();
        public IQueryable<T> QueryableNoTracking => _context.QuerySet<T>().AsNoTracking();

        public bool Any() => Queryable.Any();
        public bool Any(Expression<Func<T, bool>> where) => Queryable.Any(where);

        public Task<bool> AnyAsync() => Queryable.AnyAsync();
        public Task<bool> AnyAsync(Expression<Func<T, bool>> where) => Queryable.AnyAsync(where);

        public long Count() => Queryable.LongCount();
        public long Count(Expression<Func<T, bool>> where) => Queryable.LongCount(where);

        public Task<long> CountAsync() => Queryable.LongCountAsync();
        public Task<long> CountAsync(Expression<Func<T, bool>> where) => Queryable.LongCountAsync(where);

        public T Get(object key) => _context.DetectChangesLazyLoading(false).Set<T>().Find(key);
        public Task<T> GetAsync(object key) => _context.DetectChangesLazyLoading(false).Set<T>().FindAsync(key).AsTask();

        public IEnumerable<T> GetWithInclude(Func<T, bool> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            return includeProperties
                .Aggregate(QueryableNoTracking, (current, prop) => current.Include(prop))
                .AsEnumerable()
                .Where(predicate)
                .ToList();
        }

        public IReadOnlyList<T> GetAll() => QueryableNoTracking.ToList();
        public async Task<IReadOnlyList<T>> GetAllAsync() => await QueryableNoTracking.ToListAsync().ConfigureAwait(false);

        public IEnumerable<T> List() => Queryable.ToList();
        public async Task<IEnumerable<T>> ListAsync() => await Queryable.ToListAsync().ConfigureAwait(false);

        #endregion
    }
}