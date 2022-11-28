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
    public string? InitialDiscode { get; set; }

    public int DisId { get; set; }

    [Remote(action: "VerifyDisCode", controller: "DiscountOrders", AdditionalFields = nameof(InitialDiscode), ErrorMessage = "Mã đã tồn tại!")]
    [DisplayName("Mã Giảm Giá")]
    public string? DisCode { get; set; }

    [DisplayName("Tỉ Lệ Giảm")]
    public double? DisRate { get; set; }

    [DisplayName("Tối Đa")]
    public decimal? MaxDis { get; set; }

    [Remote(action: "VerifyDate", controller: "DiscountOrders", AdditionalFields = nameof(EndDate), ErrorMessage = "Không thể kết thúc trước khi bắt đầu!")]
    [DisplayName("Bắt Đầu")]
    public DateTime? StartDate { get; set; }

    [Remote(action: "VerifyDate", controller: "DiscountOrders", AdditionalFields = nameof(StartDate), ErrorMessage ="Không thể kết thúc trước khi bắt đầu!")]
    [DisplayName("Kết Thúc")]
    public DateTime? EndDate { get; set; }

    [Range(1,3, ErrorMessage ="Loại KH phải từ 1-3")]
    [DisplayName("Loại KH")]
    public int? CusType { get; set; }

    [DisplayName("Trạng Thái")]
    public bool? Status { get; set; } = false;

    public virtual ICollection<Order> Orders { get; } = new List<Order>();
}
