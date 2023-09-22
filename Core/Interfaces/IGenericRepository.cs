using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces;
public interface IGenericRepository<T> where T : class
{
    Task<T> GetByIdAsync(int id);
    Task<bool> ExistsAsync(Expression<Func<T, bool>> expression);
    Task<IEnumerable<T>> GetAllAsync();
    IEnumerable<T> Find(Expression<Func<T, bool>> expression);
    IEnumerable<TResult> Join<TInner, TKey, TResult>(
        IEnumerable<T> outer,
        IEnumerable<TInner> inner,
        Func<T, TKey> outerKeySelector,
        Func<TInner, TKey> innerKeySelector,
        Func<T, TInner, TResult> resultSelector
    );
    void AddRangeAsync(List<T> items);
    void UpdateRangeAsync(List<T> items);
    void RemoveRange(IEnumerable<T> items);
    void Add(T entity);
    void Remove(T entity);
    void Update(T entity);
}

