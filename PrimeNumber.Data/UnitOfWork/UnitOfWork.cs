using PrimeNumber.Data.DbContext;
using PrimeNumber.Data.Entity;
using PrimeNumber.Data.Repository;

namespace PrimeNumber.Data.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly DatabaseContext _context;
   
    public UnitOfWork(DatabaseContext context)
    {
        _context = context;
    }

    public IGenericRepository<T> GetRepository<T>() where T : class
    {
        return new GenericRepository<T>(_context);
    }

    public Task<int> SaveChangesAsync()
    {
        try
        {
            return _context.SaveChangesAsync();
        }
        catch
        {
            throw;
        }
    }

    private bool disposed = false;
    protected virtual void Dispose(bool disposing)
    {
        if (!this.disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }

        this.disposed = true;
    }
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

   
}
