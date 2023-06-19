using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using PrimeNumber.Core.Helper;
using PrimeNumber.Data.DbContext;

public static class UserRoleData
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

        #region Roles
        string[] roles = new string[] { UserRoles.Admin.ToString(), UserRoles.User.ToString() };

        foreach (string role in roles)
        {
            RoleStore<IdentityRole> roleStore = new(context);

            if (!context.Roles.Any(r => r.Name == role))
            {
                await roleStore.CreateAsync(new IdentityRole
                {
                    Name = role,
                    NormalizedName = role.ToUpperInvariant(),
                    ConcurrencyStamp = ((int)Enum.Parse(typeof(UserRoles), role)).ToString()
                });
            }
        }
        #endregion

        #region Users
        var user = new IdentityUser
        {
            Email = "onurdbnc@gmail.com",
            NormalizedEmail = "ONURDBNC@GMAIL.COM",
            UserName = "Onur",
            NormalizedUserName = "ONUR",
            PhoneNumber = "+111111111111",
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString("D")
        };

        if (!context.Users.Any(u => u.UserName == user.UserName))
        {
            var password = new PasswordHasher<IdentityUser>();
            var hashed = password.HashPassword(user, "Onur123.");
            user.PasswordHash = hashed;

            var userStore = new UserStore<IdentityUser>(context);
            await userStore.CreateAsync(user);
        }
        #endregion

        #region AssignUsersToRoles
        var _userManager = scope.ServiceProvider.GetService<UserManager<IdentityUser>>();
        if (_userManager is not null)
        {
            var getUser = await _userManager.FindByEmailAsync(user.Email);
            if (getUser is not null)
            {
                await _userManager.AddToRoleAsync(getUser, roles[0]);
            }
        }
        #endregion

        await context.SaveChangesAsync();
    }
}