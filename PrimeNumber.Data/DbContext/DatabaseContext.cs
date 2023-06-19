using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PrimeNumber.Data.Entity;

namespace PrimeNumber.Data.DbContext
{
    public class DatabaseContext : IdentityDbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public DbSet<Input> Inputs { get; set; }

        public DbSet<Result> Results { get; set; }

    }
}

