using System;
using System.Collections.Generic;

namespace Task4.Models;

public partial class Size
{
    public int Id { get; set; }

    public int Value { get; set; }

    public virtual ICollection<Pizza> Pizzas { get; set; } = new List<Pizza>();
}
