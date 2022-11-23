using System;
using System.Collections.Generic;

namespace WebBriliFresh.Models;

public partial class DiscountAll
{
    public int DisId { get; set; }

    public double? Value { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }
}
