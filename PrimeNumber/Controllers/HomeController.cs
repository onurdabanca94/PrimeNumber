using Microsoft.AspNetCore.Mvc;
using PrimeNumber.Core.Models;
using PrimeNumber.Core.Services;

namespace PrimeNumber.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICalculationPrimeNumber _calculationPrimeNumber;

        public HomeController(ICalculationPrimeNumber calculationPrimeNumber)
        {
            _calculationPrimeNumber = calculationPrimeNumber;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> AddPrime([FromBody] PrimeRequestModel model)
        {
            if (model == null || model.Prime < 2 || model.UserId == new Guid())
            {
                return BadRequest();
            }

            var getMaxPrimeNumber = await _calculationPrimeNumber.AddPrimeAsync(model.Prime, model.UserId);

            return Ok(new { maxPrime = getMaxPrimeNumber });
        }
    }
}
