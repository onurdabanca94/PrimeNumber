using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrimeNumber.Core.Services;
using PrimeNumber.Ui.Helper;

namespace PrimeNumber.Ui.Controllers
{
    [AuthenticationFilter]
    public class AdminController : Controller
    {
        private readonly ICalculationPrimeNumber _calculationPrimeNumber;

        public AdminController(ICalculationPrimeNumber calculationPrimeNumber)
        {
            _calculationPrimeNumber = calculationPrimeNumber;
        }

        public async Task<IActionResult> Index()
        {
            var results = await _calculationPrimeNumber.GetResultsAsync();
            return View(results);
        }
    }
}
