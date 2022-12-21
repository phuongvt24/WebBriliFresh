using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBriliFresh.Models;

public partial class DiscountOrder
{
    [NotMapped]
    public string? InitialDisCode { get; set; }

    public int DisId { get; set; }

    [Required(ErrorMessage ="Vui lòng điền đầy đủ thông tin")]
    [Remote(action: "VerifyDisCode", controller: "DiscountOrders", AdditionalFields = "InitialDisCode", ErrorMessage = "Mã đã tồn tại!")]
    [DisplayName("Mã Giảm Giá")]
    public string? DisCode { get; set; }

    [Range(0.05, 1, ErrorMessage = "Tỉ lệ giảm phải từ 0.05 đén 1")]
    [Required(ErrorMessage = "Vui lòng điền đầy đủ thông tin")]
    [DisplayName("Tỉ Lệ Giảm")]
    public double? DisRate { get; set; }

    [Required(ErrorMessage = "Vui lòng điền đầy đủ thông tin")]
    [DisplayName("Tối Đa")]
    public decimal? MaxDis { get; set; }

    [Required(ErrorMessage = "Vui lòng điền đầy đủ thông tin")]
    [Remote(action: "VerifyDate", controller: "DiscountOrders", AdditionalFields = nameof(EndDate), ErrorMessage = "Không thể kết thúc trước khi bắt đầu!")]
    [DisplayName("Bắt Đầu")]
    public DateTime? StartDate { get; set; }

    [Required(ErrorMessage = "Vui lòng điền đầy đủ thông tin")]
    [Remote(action: "VerifyDate", controller: "DiscountOrders", AdditionalFields = nameof(StartDate), ErrorMessage ="Không thể kết thúc trước khi bắt đầu!")]
    [DisplayName("Kết Thúc")]
    public DateTime? EndDate { get; set; }

    [Required(ErrorMessage = "Vui lòng điền đầy đủ thông tin")]
    [Range(1,3, ErrorMessage ="Loại KH phải từ 1-3")]
    [DisplayName("Loại KH")]
    public int? CusType { get; set; }


    [Required(ErrorMessage = "Vui lòng điền đầy đủ thông tin")]
    [DisplayName("Trạng Thái")]
    public bool? Status { get; set; } = false;

    public virtual ICollection<Order> Orders { get; } = new List<Order>();
    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();
}
