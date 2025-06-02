using System;
using System.Collections.Generic;

namespace Task4.Models;

public partial class DoughType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Pizza> Pizzas { get; set; } = new List<Pizza>();
}
