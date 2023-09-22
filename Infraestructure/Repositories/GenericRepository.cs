using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories;
public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly TiendaContext _context;
    public GenericRepository(TiendaContext context)
    {
        _context = context;
    }
    public virtual void Add(T entity)
    {
        _context.Set<T>().Add(entity);
    }

    public virtual void AddRangeAsync(List<T> items)
    {
        _context.Set<T>().AddRangeAsync(items);
    }

    public virtual IEnumerable<T> Find(Expression<Func<T, bool>> expression)
    {
        return _context.Set<T>().Where(expression);
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _context.Set<T>().ToListAsync();
    }

    public virtual async Task<bool> ExistsAsync(Expression<Func<T, bool>> expression)
    {
        return await _context.Set<T>().AnyAsync(expression);
    }
    public virtual async Task<T> GetByIdAsync(int id)
    {
        return await _context.Set<T>().FindAsync(id);
    }

    public void RemoveRange(IEnumerable<T> items)
    {
        _context.Set<T>().RemoveRange(items);
    }
    public void UpdateRangeAsync(List<T> items)
    {
        _context.Set<T>().UpdateRange(items);
    }
    public virtual void Update(T entity)
    {
        _context.Set<T>()
            .Update(entity);
    }
    public virtual void Remove(T entity)
    {
        _context.Set<T>().Remove(entity);
    }

    public IEnumerable<TResult> Join<TInner, TKey, TResult>(IEnumerable<T> outer, IEnumerable<TInner> inner,
                                                            Func<T, TKey> outerKeySelector, Func<TInner,
                                                            TKey> innerKeySelector, Func<T, TInner,
                                                            TResult> resultSelector)
    {
        return outer.Join(
            inner,
            outerKeySelector,
            innerKeySelector,
            resultSelector
        );
    }
}
