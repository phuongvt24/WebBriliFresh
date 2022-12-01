using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WebBriliFresh.Models;

public partial class Stock
{
    [Key] [Column(Order = 0)]
    public int StoreId { get; set; }
    [Key] [Column(Order = 1)]
    public int ProId { get; set; }
    [Required (ErrorMessage="Chưa nhập số lượng sản phẩm!")]
    [Range(0, Double.MaxValue,ErrorMessage = "Số lượng sản phẩm không thể là số âm!")]
    public int? Quantity { get; set; }
    public virtual Product Pro { get; set; } = null!;
    public virtual Store Store { get; set; } = null!;
}
