using System;
using System.Collections.Generic;

namespace some_products_app2.Models;

public partial class PartnerType
{
    public int PartnerTypeId { get; set; }

    public string? PartnerTypeName { get; set; }

    public virtual ICollection<PartnerImport> PartnerImports { get; set; } = new List<PartnerImport>();
}
