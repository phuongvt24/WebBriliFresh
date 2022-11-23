using System;
using System.Collections.Generic;

namespace WebBriliFresh.Models;

public partial class DiscountProduct
{
    public int DisId { get; set; }

    public int? ProId { get; set; }

    public double? Value { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public virtual Product? Pro { get; set; }
}
