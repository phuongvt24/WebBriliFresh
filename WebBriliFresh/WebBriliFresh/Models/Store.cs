using System;
using System.Collections.Generic;

namespace WebBriliFresh.Models;

public partial class Store
{
    public int StoreId { get; set; }

    public string? City { get; set; }

    public string? District { get; set; }

    public string? Ward { get; set; }

    public string? SpecificAddress { get; set; }

    public virtual ICollection<DiscountStore> DiscountStores { get; } = new List<DiscountStore>();

    public virtual ICollection<Employee> Employees { get; } = new List<Employee>();

    public virtual ICollection<Order> Orders { get; } = new List<Order>();

    public virtual ICollection<Stock> Stocks { get; } = new List<Stock>();
}
