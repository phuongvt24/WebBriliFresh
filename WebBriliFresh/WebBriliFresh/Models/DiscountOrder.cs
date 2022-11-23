using System;
using System.Collections.Generic;

namespace WebBriliFresh.Models;

public partial class DiscountOrder
{
    public int DisId { get; set; }

    public string? DisCode { get; set; }

    public double? DisRate { get; set; }

    public decimal? MaxDis { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public int? CusType { get; set; }

    public virtual ICollection<Order> Orders { get; } = new List<Order>();
}
