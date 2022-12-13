using System;
using System.Collections.Generic;

namespace WebBriliFresh.Models;

public partial class Reward
{
    public int RewardId { get; set; }

    public int? CusType { get; set; } = 1;

    public decimal? Point { get; set; } = 0;

    public virtual ICollection<Customer> Customers { get; } = new List<Customer>();
}
