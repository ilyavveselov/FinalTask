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

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var sizes = await _pizzaRepository.GetAvaiableSizes();
            var types = await _pizzaRepository.GetAvaiableDoughTypes();
            ViewBag.AllSizes = sizes;
            ViewBag.AllDoughTypes = types;

            var pizza = await _pizzaRepository.GetPizzaById(id);
            if (pizza == null)
                return NotFound();

            return PartialView("PizzaPartials/_Form", pizza);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.AllSizes = await _pizzaRepository.GetAvaiableSizes();
            ViewBag.AllDoughTypes = await _pizzaRepository.GetAvaiableDoughTypes();
            return PartialView("PizzaPartials/_Form", new PizzaModel(new Pizza()));
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
