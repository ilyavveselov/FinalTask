using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Task.Models;
using Task.Repositories;

namespace Task.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPizzaRepository _pizzaRepository;

        public HomeController(ILogger<HomeController> logger, IPizzaRepository pizzaRepository)
        {
            _logger = logger;
            _pizzaRepository = pizzaRepository;
        }

        public async Task<IActionResult> Index()
        {
            var pizzas = await _pizzaRepository.GetAllPizzas();
            return View(pizzas);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
