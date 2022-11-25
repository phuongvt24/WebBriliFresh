using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebBriliFresh.Models;

public partial class DiscountOrder
{
    public int DisId { get; set; }

    public string? DisCode { get; set; }

    public double? DisRate { get; set; }

    public decimal? MaxDis { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    [Range(1,3)]
    public int? CusType { get; set; }

    public int? Status { get; set; }

    public virtual ICollection<Order> Orders { get; } = new List<Order>();
}
