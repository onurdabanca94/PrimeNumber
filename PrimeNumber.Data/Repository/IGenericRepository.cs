using Microsoft.EntityFrameworkCore;
using PrimeNumber.Data.Entity;
using System.Linq.Expressions;

namespace PrimeNumber.Data.Repository;

public interface IGenericRepository<T> where T : class
{
    Task<IQueryable<T>> GetAllAsync(Expression<Func<T, bool>> predicate);
    Task<List<T>> GetAllAsync();

    Task<T> GetByIdAsync(int id);

    Task<T> GetAsync(Expression<Func<T, bool>> predicate);

    Task AddAsync(T entity);

    Task Update(T entity);

    Task DeleteAsync(int id);

}