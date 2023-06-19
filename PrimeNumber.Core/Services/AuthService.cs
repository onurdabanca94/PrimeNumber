using Microsoft.AspNetCore.Identity;
using PrimeNumber.Core.Helper;
using PrimeNumber.Core.Models;

namespace PrimeNumber.Core.Services;

public class AuthService: IAuthService
{

    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public AuthService(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    public async Task<ResponseModel> Login(LoginRequest request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
            return new ResponseModelError("User not found!");

        var signInResult = await _signInManager.PasswordSignInAsync(user, request.Password, false, false);

        if (signInResult.Succeeded)
        {
            await _userManager.ResetAccessFailedCountAsync(user);
            var userRole = _userManager
                .GetRolesAsync(user).Result
                .FirstOrDefault()
                    ?? UserRoles.User.ToString();

            var token = TokenManagerService.CreateToken(GetUserClaim(user, userRole));

            return new ResponseModelSuccess(token);
        }
        else
        {
            if (signInResult.IsLockedOut)
                return new ResponseModelError("Your account is locked!");

            await _userManager.AccessFailedAsync(user);
            int failcount = await _userManager.GetAccessFailedCountAsync(user);
            if (failcount == 0)
            {
                await _userManager.SetLockoutEndDateAsync(user, new DateTimeOffset(DateTime.Now.AddMinutes(5)));
                return new ResponseModelError("You have made 3 unsuccessful logins. your account is locked!");
            }
            else
            {
                return new ResponseModelError("Email or password is incorrect!");
            }
        }

        throw new NotImplementedException();
    }

    private static Dictionary<string, object> GetUserClaim(IdentityUser user, string role)
    {
        var userInfo = new Dictionary<string, object>
            {
                { TokenManagerService.UserId    , user.Id},
                { TokenManagerService.UserName  , user.UserName ?? "System"},
                { TokenManagerService.Roles     ,  role},
                { TokenManagerService.LoginDate , DateTime.Now },
                { TokenManagerService.ExpireDate, DateTime.Now.AddHours(24) }
            };
        return userInfo;
    }

}
