using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebBriliFresh.Models;

public partial class Product
{
    public int ProId { get; set; }

    public string? ProName { get; set; }

    public decimal? Price { get; set; }

    public int? TypeId { get; set; }

    public string? Source { get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
    public DateTime? StartDate { get; set; }

    public string? Des { get; set; }

    public string? Unit { get; set; }

    public int? IsDeleted { get; set; }

    public virtual ICollection<DiscountProduct> DiscountProducts { get; } = new List<DiscountProduct>();

    public virtual ICollection<Feedback> Feedbacks { get; } = new List<Feedback>();

    public virtual ICollection<OrderDetail> OrderDetails { get; } = new List<OrderDetail>();

    public virtual ICollection<ProductImage> ProductImages { get; } = new List<ProductImage>();

    public virtual ICollection<QnA> QnAs { get; } = new List<QnA>();

    public virtual ICollection<Stock> Stocks { get; } = new List<Stock>();

    public virtual Type? Type { get; set; }
}
