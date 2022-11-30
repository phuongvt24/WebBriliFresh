using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBriliFresh.Models;

public partial class Product
{
    public int ProId { get; set; }
    [Required(ErrorMessage ="Nhập tên cho sản phẩm!")]
    public string? ProName { get; set; }

    //[DataType(DataType.Currency)]
    //[DisplayFormat(DataFormatString = "{0:0 vnđ}", ApplyFormatInEditMode = true)]
    [Required(ErrorMessage = "Nhập giá cho sản phẩm!")]
    public decimal? Price { get; set; }
    public string PriceString => $"{Price:N}";
    public int? TypeId { get; set; }

    [Required(ErrorMessage = "Chọn nguồn gốc cho sản phẩm!")]
    public string? Source { get; set; }

    [Required(ErrorMessage = "Chọn ngày nhập sản phẩm!")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
    public DateTime? StartDate { get; set; }

    public string? Des { get; set; }

    public string? Unit { get; set; }

    public int? IsDeleted { get; set; }
    [NotMapped]
    public  IFormFile? File { get; set; }
    [NotMapped]
    public  IList<IFormFile>? Files { get; set; }

    public virtual ICollection<DiscountProduct> DiscountProducts { get; } = new List<DiscountProduct>();

    public virtual ICollection<Feedback> Feedbacks { get; } = new List<Feedback>();

    public virtual ICollection<OrderDetail> OrderDetails { get; } = new List<OrderDetail>();

    public virtual ICollection<ProductImage> ProductImages { get; } = new List<ProductImage>();

    public virtual ICollection<QnA> QnAs { get; } = new List<QnA>();

    public virtual ICollection<Stock> Stocks { get; } = new List<Stock>();

    public virtual Type? Type { get; set; }
}
