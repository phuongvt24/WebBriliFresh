using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebBriliFresh.Models;

public partial class DiscountOrder
{
    public int DisId { get; set; }

    [DisplayName("Mã Giảm Giá")]
    public string? DisCode { get; set; }

    [DisplayName("Tỉ Lệ Giảm")]
    public double? DisRate { get; set; }

    [DisplayName("Tối Đa")]
    public decimal? MaxDis { get; set; }

    [DisplayName("Bắt Đầu")]
    public DateTime? StartDate { get; set; }

    [DisplayName("Kết Thúc")]
    public DateTime? EndDate { get; set; }

    [Range(1,3, ErrorMessage ="Loại KH phải từ 1-3")]
    [DisplayName("Loại KH")]
    public int? CusType { get; set; }

    [DisplayName("Trạng Thái")]
    public bool? Status { get; set; }

    public virtual ICollection<Order> Orders { get; } = new List<Order>();
}
