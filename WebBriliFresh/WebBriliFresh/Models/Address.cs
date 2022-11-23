using System;
using System.Collections.Generic;

namespace WebBriliFresh.Models;

public partial class Address
{
    public int AddId { get; set; }

    public int? CusId { get; set; }

    public string? City { get; set; }

    public string? District { get; set; }

    public string? Ward { get; set; }

    public string? SpecificAddress { get; set; }

    public int? Default { get; set; }

    public virtual Customer? Cus { get; set; }

    public virtual ICollection<Order> Orders { get; } = new List<Order>();
}
