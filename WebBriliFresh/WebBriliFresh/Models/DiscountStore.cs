using System;
using System.Collections.Generic;

namespace WebBriliFresh.Models;

public partial class DiscountStore
{
    public int DisId { get; set; }

    public int? StoreId { get; set; }

    public double? Value { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public virtual Store? Store { get; set; }
}
