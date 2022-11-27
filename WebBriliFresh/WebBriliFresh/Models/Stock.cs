using System;
using System.Collections.Generic;

namespace WebBriliFresh.Models;

public partial class Stock
{
    public int StoreId { get; set; }

    [Key] [Column(Order = 0)]
    public int StoreId { get; set; }
    [Key] [Column(Order = 1)]
    public int ProId { get; set; }

    public int? Quantity { get; set; }

    public virtual Product Pro { get; set; } = null!;

    public virtual Product Pro { get; set; } = null!;
    public virtual Store Store { get; set; } = null!;
}
