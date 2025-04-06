using System;
using System.Collections.Generic;

namespace some_products_app2.Models;

public partial class Director
{
    public int DirectorId { get; set; }

    public string? DirectorSurname { get; set; }

    public string? DirectorName { get; set; }

    public string? DirectorPatronymic { get; set; }

    public string? DirectorEmail { get; set; }

    public string? DirectorPhone { get; set; }

    public virtual ICollection<PartnerImport> PartnerImports { get; set; } = new List<PartnerImport>();
}
