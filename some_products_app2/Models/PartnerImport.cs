using System;
using System.Collections.Generic;

namespace some_products_app2.Models;

public partial class PartnerImport
{
    public int PartnerImportId { get; set; }

    public int? PartnerTypeId { get; set; }

    public string? PartnerImportName { get; set; }

    public int? DirectorId { get; set; }

    public string? PartnerImportIndex { get; set; }

    public string? PartnerImportOblast { get; set; }

    public string? PartnerImportCity { get; set; }

    public string? PartnerImportStreet { get; set; }

    public string? PartnerImportHouse { get; set; }

    public string? PartnerImport1 { get; set; }

    public float? PartnerImportRating { get; set; }

    public virtual Director? Director { get; set; }

    public virtual PartnerType? PartnerType { get; set; }

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();

    public int CalculateDiscount
    {
        get {
            int countSales = 0;
            int discount = 0;

            countSales = (int)Sales.Sum(o => o.ProductCount);

            if (countSales < 10000)
                discount = 0;

            else if (countSales >= 10000 && countSales < 50000)
                discount = 5;

            else if (countSales >= 50000 && countSales < 300000)
                discount = 10;

            else
                discount = 15;

            return discount;
        }
    }
}
