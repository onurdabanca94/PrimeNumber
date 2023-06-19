using Microsoft.AspNetCore.Mvc;
using PrimeNumber.Core.Models;
using PrimeNumber.Core.Services;

namespace PrimeNumber.Ui.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest model)
        {

            var token = await _authService.Login(model, CancellationToken.None);
            if (string.IsNullOrEmpty(token.Message))
            {
                ViewBag.Message = "Login Failed";
                return View(model);
            }
            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(1),
                Path = "/"
            };
            Response.Cookies.Append("Auth", token.Message, cookieOptions);

            return RedirectToAction("Index", "Admin", "");
        }

        public async Task<IActionResult> Logout()
        {
            Response.Cookies.Delete("Auth");
            return RedirectToAction("Index", "Home", "");
        }
    }
}
