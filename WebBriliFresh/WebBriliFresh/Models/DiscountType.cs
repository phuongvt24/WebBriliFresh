using System;
using System.Collections.Generic;

namespace WebBriliFresh.Models;

public partial class DiscountType
{
    public int DisId { get; set; }

    public int? TypeId { get; set; }

    public double? Value { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public virtual Type? Type { get; set; }
}
