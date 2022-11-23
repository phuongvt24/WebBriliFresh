using System;
using System.Collections.Generic;

namespace WebBriliFresh.Models;

public partial class Type
{
    public int TypeId { get; set; }

    public string? SubType { get; set; }

    public string? MainType { get; set; }

    public virtual ICollection<DiscountType> DiscountTypes { get; } = new List<DiscountType>();

    public virtual ICollection<Product> Products { get; } = new List<Product>();
}
