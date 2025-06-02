using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task.Models;

namespace Task.Repositories
{
    public class PizzaRepository : IPizzaRepository
    {
        private readonly PizzasStoreContext _context;

        public PizzaRepository(PizzasStoreContext context)
        {
            _context = context;
        }
        public async Task<List<PizzaModel>> GetAllPizzas()
        {
            var pizzasFromDb = await _context.Pizzas.Include("Sizes").Include("DoughTypes").ToListAsync(); ;
            var pizzas = pizzasFromDb.Select(p => new PizzaModel(p)).ToList();
            return pizzas;
        }

        public async Task<List<Size>> GetAvaiableSizes()
        {
            var sizes = await _context.Sizes.Include(s => s.Pizzas).OrderBy(s => s.Value).ToListAsync();
            return sizes;
        }
        public async Task<List<DoughType>> GetAvaiableDoughTypes()
        {
            var types = await _context.DoughTypes.Include(s => s.Pizzas).OrderBy(t => t.Name).ToListAsync();
            return types;
        }
        public async Task<PizzaModel> GetPizzaById(int id)
        {
            var foundedPizzaFromDb = await _context.Pizzas.Include("Sizes").Include("DoughTypes").FirstOrDefaultAsync(p => p.Id == id);
            if (foundedPizzaFromDb != null)
            {
                var foundedPizza = new PizzaModel(foundedPizzaFromDb);
                return foundedPizza;
            }
            else
            {
                return null;
            }
        }

        public async System.Threading.Tasks.Task CreatePizza(PizzaModel model)
        {
            var sizes = await _context.Sizes
                .Where(s => model.Sizes.Contains(s.Value))
                .ToListAsync();

            var doughTypes = await _context.DoughTypes
                .Where(d => model.Types.Contains(d.Name))
                .ToListAsync();

            var pizza = new Pizza
            {
                Name = model.Name,
                Image = model.Image,
                Description = model.Description,
                Price = model.Price,
                Weight = model.Weight,
                ShowHalf = model.ShowHalf,
                CanHalf = model.CanHalf,
                IsHit = model.IsHit,
                Sizes = sizes,
                DoughTypes = doughTypes
            };

            _context.Pizzas.Add(pizza);
            await _context.SaveChangesAsync();
        }
        public async System.Threading.Tasks.Task EditPizza(PizzaModel pizza)
        {
            if (pizza == null) return;

            var pizzaFromDb = await _context.Pizzas
                .Include(p => p.Sizes)
                .Include(p => p.DoughTypes)
                .FirstOrDefaultAsync(p => p.Id == pizza.Id);

            if (pizzaFromDb != null)
            {
                pizzaFromDb.Name = pizza.Name;
                pizzaFromDb.Image = pizza.Image;
                pizzaFromDb.Description = pizza.Description;
                pizzaFromDb.Price = pizza.Price;
                pizzaFromDb.Weight = pizza.Weight;
                pizzaFromDb.CanHalf = pizza.CanHalf;
                pizzaFromDb.ShowHalf = pizza.ShowHalf;
                pizzaFromDb.IsHit = pizza.IsHit;

                pizzaFromDb.Sizes.Clear();
                pizzaFromDb.DoughTypes.Clear();

                if (pizza.Sizes != null && pizza.Sizes.Any())
                {
                    var sizesFromDb = await _context.Sizes
                        .Where(s => pizza.Sizes.Contains(s.Value))
                        .ToListAsync();

                    foreach (var size in sizesFromDb)
                    {
                        pizzaFromDb.Sizes.Add(size);
                    }
                }

                if (pizza.Types != null && pizza.Types.Any())
                {
                    var doughsFromDb = await _context.DoughTypes
                        .Where(d => pizza.Types.Contains(d.Name))
                        .ToListAsync();

                    foreach (var dough in doughsFromDb)
                    {
                        pizzaFromDb.DoughTypes.Add(dough);
                    }
                }

                await _context.SaveChangesAsync();
            }
        }

        public async System.Threading.Tasks.Task DeletePizza(PizzaModel pizza)
        {
            var pizzaToDelete = await _context.Pizzas
                .Include("Sizes")
                .Include("DoughTypes")
                .FirstOrDefaultAsync(p => p.Id == pizza.Id);
            if (pizzaToDelete != null)
            {
                var sizesToDelete = await _context.Sizes
                         .Where(s => pizza.Sizes.Contains(s.Value))
                         .ToListAsync();
                var typesToDelete = await _context.DoughTypes
                         .Where(t => pizza.Types.Contains(t.Name))
                         .ToListAsync();
                pizzaToDelete.Sizes.Clear();
                pizzaToDelete.DoughTypes.Clear();

                _context.Pizzas.Remove(pizzaToDelete);
                await _context.SaveChangesAsync();
            }
        }

    }
}
