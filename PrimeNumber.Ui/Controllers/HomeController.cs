using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PrimeNumber.Ui.Models;
using System.Diagnostics;
using PrimeNumber.Core.Services;

namespace PrimeNumber.Ui.Controllers
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
    }
}