using System;
using System.Collections.Generic;

namespace some_products_app2.Models;

public partial class Sale
{
    public int SaleId { get; set; }

    public int? Article { get; set; }

    public int? PartnerImportId { get; set; }

    public int? ProductCount { get; set; }

    public DateOnly? SaleDate { get; set; }

    public virtual Product? ArticleNavigation { get; set; }

    public virtual PartnerImport? PartnerImport { get; set; }
}
