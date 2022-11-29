using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace WebBriliFresh.Models;

public partial class DiscountProduct
{
    public int DisId { get; set; }

    public int? ProId { get; set; }

    [DisplayName("Tỉ lệ giảm")]
    public double? Value { get; set; }

    [DisplayName("Bắt đầu")]
    public DateTime? StartDate { get; set; }

    [DisplayName("Kết thúc")]
    public DateTime? EndDate { get; set; }

    [DisplayName("Trạng thái")]
    public bool? Status { get; set; }

    public virtual Product? Pro { get; set; }
}
