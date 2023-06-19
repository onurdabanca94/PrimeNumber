using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PrimeNumber.Core.Services;
using PrimeNumber.Data.DbContext;
using PrimeNumber.Data.Repository;
using PrimeNumber.Data.UnitOfWork;

namespace PrimeNumber
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();
            builder.Services.AddMvc().AddRazorRuntimeCompilation();
            builder.Services.AddHttpContextAccessor();

            var cnnString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(cnnString));

            builder.Services.AddIdentity<IdentityUser, IdentityRole>(opt =>
            {
                opt.User.RequireUniqueEmail = true;
                opt.Password.RequiredLength = 6;
                opt.Password.RequireNonAlphanumeric = true;
                opt.Password.RequireDigit = true;
                opt.Password.RequireLowercase = true;
                opt.Password.RequireUppercase = true;
                opt.SignIn.RequireConfirmedEmail = false;

                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                opt.Lockout.MaxFailedAccessAttempts = 3;
                opt.Lockout.AllowedForNewUsers = true;
            })
            .AddEntityFrameworkStores<DatabaseContext>()
            .AddDefaultTokenProviders();

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<ICalculationPrimeNumber, CalculationPrimeNumber>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddCors();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            if(app.Environment.IsDevelopment())
            {
                app.UseCors(p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}