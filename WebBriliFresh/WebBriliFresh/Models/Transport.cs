using System;
using System.Collections.Generic;

namespace WebBriliFresh.Models;

public partial class Transport
{
    public int TransId { get; set; }

    public DateTime? ShippingDate { get; set; }

    public string? Transporter { get; set; }

    public int Status { get; set; }

    public decimal Fee { get; set; }

    public int Type { get; set; }

    public virtual ICollection<Order> Orders { get; } = new List<Order>();
}
