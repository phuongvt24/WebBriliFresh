using System;
using System.Collections.Generic;

namespace WebBriliFresh.Models;

public partial class OrderDetail
{
    public int OrderId { get; set; }

    public int ProId { get; set; }

    public int? Quantity { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Product Pro { get; set; } = null!;
}
