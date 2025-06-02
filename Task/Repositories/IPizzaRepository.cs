using Task.Models;

namespace Task.Repositories
{
    public interface IPizzaRepository
    {
        Task<List<PizzaModel>> GetAllPizzas();

        Task<PizzaModel> GetPizzaById(int id);

        System.Threading.Tasks.Task CreatePizza(PizzaModel pizza);

        System.Threading.Tasks.Task EditPizza(PizzaModel model);

        Task<List<Size>> GetAvaiableSizes();

        Task<List<DoughType>> GetAvaiableDoughTypes();

        System.Threading.Tasks.Task DeletePizza(PizzaModel pizza);
    }
}
