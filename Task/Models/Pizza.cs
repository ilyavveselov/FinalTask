using System;
using System.Collections.Generic;

namespace Task4.Models;

public partial class Pizza
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Image { get; set; } = string.Empty;

    public string Description { get; set; } = null!;

    public int Price { get; set; }

    public int Weight { get; set; }

    public bool ShowHalf { get; set; }

    public bool CanHalf { get; set; }

    public bool IsHit { get; set; }

    public virtual ICollection<DoughType> DoughTypes { get; set; } = new List<DoughType>();

    public virtual ICollection<Size> Sizes { get; set; } = new List<Size>();
}
