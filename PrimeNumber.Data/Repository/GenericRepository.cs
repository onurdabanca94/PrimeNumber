using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PrimeNumber.Data.DbContext;
using System.Data;
using System;
using System.Linq.Expressions;

namespace PrimeNumber.Data.Repository;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly DatabaseContext _context;
    private readonly DbSet<T> _dbSet;

    public GenericRepository(DatabaseContext dbContext)
    {
        _context = dbContext;
        _dbSet = dbContext.Set<T>();
    }

    public Task<IQueryable<T>> GetAllAsync(Expression<Func<T, bool>> predicate) => Task.Run(() => _dbSet.Where(predicate));
    public async Task<List<T>> GetAllAsync() =>  await _dbSet.ToListAsync<T>();

    public async Task<T> GetByIdAsync(int id) => await _dbSet.FindAsync(id);

    public async Task<T> GetAsync(Expression<Func<T, bool>> predicate) => await _dbSet.FirstOrDefaultAsync(predicate);

    public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);

    public async Task Update(T entity)
    {
        _dbSet.Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
    }
    public async Task DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        _context.Remove(entity);
    }
}
