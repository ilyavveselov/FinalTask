using Microsoft.AspNetCore.Mvc;
using Task.Models;
using Task.Repositories;

namespace Task.Controllers
{
    [ApiController]
    [Route("api/pizza")]
    public class PizzasApiController : ControllerBase
    {
        private readonly ILogger<PizzasApiController> _logger;
        private readonly IPizzaRepository _pizzaRepository;

        public PizzasApiController(ILogger<PizzasApiController> logger, IPizzaRepository pizzaRepository)
        {
            _logger = logger;
            _pizzaRepository = pizzaRepository;
        }

        [HttpGet("sizes")]
        public async Task<IActionResult> GetAvaiableSizes()
        {
            var sizes = await _pizzaRepository.GetAvaiableSizes();
            return Ok(sizes.Select(s => s.Value));
        }

        [HttpGet("types")]
        public async Task<IActionResult> GetAvaiableTypes()
        {
            var types = await _pizzaRepository.GetAvaiableDoughTypes();
            return Ok(types.Select(s => s.Name));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var pizzas = await _pizzaRepository.GetAllPizzas();
            return Ok(pizzas);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPizzaById(int id)
        {
            var pizza = await _pizzaRepository.GetPizzaById(id);
            if (pizza == null)
            {
                _logger.LogError("Пицца не найдена");
                return NotFound();
            }
            return Ok(pizza);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PizzaModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _pizzaRepository.CreatePizza(model);
            return CreatedAtAction(nameof(GetPizzaById), new { id = model.Id }, model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] PizzaModel model)
        {
            if (id != model.Id)
            {
                return BadRequest("ID в URL не совпадает с ID пиццы");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingPizza = await _pizzaRepository.GetPizzaById(id);
            if (existingPizza == null)
            {
                return NotFound();
            }

            await _pizzaRepository.EditPizza(model);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var pizza = await _pizzaRepository.GetPizzaById(id);
            if (pizza == null)
            {
                return NotFound();
            }

            await _pizzaRepository.DeletePizza(pizza);
            return NoContent();
        }

        [HttpGet("error")]
        public IActionResult Error()
        {
            return Problem(detail: "An error occurred.", instance: HttpContext.TraceIdentifier);
        }
    }
}