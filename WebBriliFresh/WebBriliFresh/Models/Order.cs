using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebBriliFresh.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int? AddId { get; set; }

    public int TransId { get; set; }

    public int? DisId { get; set; }

    public int? StoreId { get; set; }

    public int? CusId { get; set; }
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
    public DateTime? OrderDate { get; set; }

    public decimal SubTotal { get; set; }
    public string SubTotalString => $"{SubTotal:N}";

    public decimal OrderTotal { get; set; }
    public string OrderTotalString => $"{OrderTotal:N}";

    public int PayBy { get; set; }

    public int Status { get; set; }

    public virtual Address? Add { get; set; }

    public virtual DiscountOrder? Dis { get; set; }

    public virtual ICollection<Feedback> Feedbacks { get; } = new List<Feedback>();

    public virtual ICollection<OrderDetail> OrderDetails { get; } = new List<OrderDetail>();

    public virtual Store? Store { get; set; }

    public virtual Transport? Trans { get; set; }
    public virtual Customer? Cus { get; set; }
}
