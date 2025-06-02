using System.ComponentModel.DataAnnotations;

namespace Task4.Models
{
    public class PizzaModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Пицца обязательно должна иметь название")]
        [Display(Name="Название")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Путь до изображения")]
        public string? Image { get; set; } = string.Empty;

        [Required(ErrorMessage = "Пицца обязательно должна иметь описание")]
        [Display(Name = "Описание")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Укажите размеры")]
        [MinLength(1, ErrorMessage = "Должен быть хотя бы один размер")]
        [Display(Name = "Размеры")]
        public List<int> Sizes { get; set; } = new();

        [Display(Name = "Типы теста")]
        public List<string> Types { get; set; } = new();

        [Required(ErrorMessage = "Пицца обязательно должна иметь цену")]
        [Range(100, int.MaxValue, ErrorMessage = "Цена должна быть от 100 руб.")]
        [Display(Name = "Цена")]
        public int Price { get; set; }

        [Display(Name = "Вес")]
        public int Weight { get; set; } = 100;

        [Display(Name = "Показывать возможность выбрать половину?")]
        public bool ShowHalf { get; set; } = false;

        [Display(Name = "Может ли быть разделена?")]
        public bool CanHalf { get; set; } = false;

        [Display(Name = "Хит?")]
        public bool IsHit { get; set; } = false;

        public PizzaModel() { }
        public PizzaModel(Pizza pizza)
        {
            Id = pizza.Id;
            Name = pizza.Name;
            Image = pizza.Image;
            Description = pizza.Description;
            Sizes = pizza.Sizes.Select(s => s.Value).ToList();
            Types = pizza.DoughTypes.Select(s => s.Name).ToList();
            Price = pizza.Price;
            Weight = pizza.Weight;
            ShowHalf = pizza.ShowHalf;
            CanHalf = pizza.CanHalf;
            IsHit = pizza.IsHit;
        }
    }
}
