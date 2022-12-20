using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBriliFresh.Models;

[PrimaryKey("DiscountOrdersDisId", "CustomersCusId")]
public partial class CustomerDiscountOrder
{
    [ForeignKey("DiscountOrder")]
    [DisplayName("Mã giảm giá")]
    public int DiscountOrdersDisId { get; set; }
    public DiscountOrder? DiscountOrder { get; set; }
    [ForeignKey("Customer")]
    [DisplayName("Khách hàng")]
    public int CustomersCusId { get; set; }
    public Customer? Customer { get; set; }

    [Required]
    [DisplayName("Trạng thái")]
    public Boolean IsDeleted { get; set; } = false;
    
}
