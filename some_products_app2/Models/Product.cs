using System;
using System.Collections.Generic;

namespace some_products_app2.Models;

public partial class Product
{
    public int Article { get; set; }

    public int? ProductTypeId { get; set; }

    public string? ProductName { get; set; }

    public float? ProductMinPrice { get; set; }

    public virtual ProductType? ProductType { get; set; }

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();
}
