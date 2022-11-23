using System;
using System.Collections.Generic;

namespace WebBriliFresh.Models;

public partial class Reward
{
    public int RewardId { get; set; }

    public int? CusType { get; set; }

    public decimal? Point { get; set; }

    public virtual ICollection<Customer> Customers { get; } = new List<Customer>();
}
