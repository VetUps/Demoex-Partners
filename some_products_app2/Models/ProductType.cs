using System;
using System.Collections.Generic;

namespace some_products_app2.Models;

public partial class ProductType
{
    public int ProductTypeId { get; set; }

    public string? ProductTypeName { get; set; }

    public float? ProductTypeCoeff { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
