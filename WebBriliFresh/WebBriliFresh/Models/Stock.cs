using System;
using System.Collections.Generic;

namespace WebBriliFresh.Models;

public partial class Stock
{
    public int StoreId { get; set; }

    public int ProId { get; set; }

    public int? Quantity { get; set; }

    public virtual Product Pro { get; set; } = null!;

    public virtual Store Store { get; set; } = null!;
}
