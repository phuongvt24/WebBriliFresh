using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebBriliFresh.Models;

public partial class DiscountStore
{
    public int DisId { get; set; }

    [Required(ErrorMessage = "Vui lòng điền đầy đủ thông tin")]
    [DisplayName("Mã Cửa Hàng")]
    public int? StoreId { get; set; }

    [DisplayName("Tỉ lệ giảm")]
    [Required(ErrorMessage = "Vui lòng điền đầy đủ thông tin")]
    [Range(0.05, 1, ErrorMessage = "Tỉ lệ giảm phải từ 0.05 đén 1")]
    public double? Value { get; set; }

    [DisplayName("Bắt đầu")]
    [Required(ErrorMessage = "Vui lòng điền đầy đủ thông tin")]
    [Remote(action: "VerifyDate", controller: "DiscountProducts", AdditionalFields = nameof(EndDate), ErrorMessage = "Không thể kết thúc trước khi bắt đầu!")]
    public DateTime? StartDate { get; set; }

    [DisplayName("Kết thúc")]
    [Remote(action: "VerifyDate", controller: "DiscountProducts", AdditionalFields = nameof(EndDate), ErrorMessage = "Không thể kết thúc trước khi bắt đầu!")]
    [Required(ErrorMessage = "Vui lòng điền đầy đủ thông tin")]
    public DateTime? EndDate { get; set; }

    [DisplayName("Trạng thái")]
    [Required(ErrorMessage = "Vui lòng điền đầy đủ thông tin")]
    public bool? Status { get; set; }

    public virtual Store? Store { get; set; }
}
