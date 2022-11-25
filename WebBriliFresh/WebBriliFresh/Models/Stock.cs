using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WebBriliFresh.Models;

public partial class Stock
{
    [Key, Column(Order = 1)]
    public int StoreId { get; set; }
    [Key, Column(Order = 2)]
    public int ProId { get; set; }
    [Required]
    public int? Quantity { get; set; }

    public virtual Product Pro { get; set; } = null!;

    public virtual Store Store { get; set; } = null!;
}
