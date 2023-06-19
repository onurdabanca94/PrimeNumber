using PrimeNumber.Data.Repository;

namespace PrimeNumber.Data.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    IGenericRepository<T> GetRepository<T>() where T : class;
    Task<int> SaveChangesAsync();
}
