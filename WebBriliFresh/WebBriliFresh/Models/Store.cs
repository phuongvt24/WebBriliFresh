using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace WebBriliFresh.Models;

public partial class Store
{
    public int StoreId { get; set; }
    [Required(ErrorMessage = "Chưa chọn Tỉnh/Thành!")]
    public string? City { get; set; }
    [Required(ErrorMessage = "Chưa chọn Quận/Huyện!")]
    public string? District { get; set; }
    [Required(ErrorMessage = "Chưa chọn Phường/Xã!")]
    public string? Ward { get; set; }
    
    public string? SpecificAddress { get; set; }

    public int? IsDeleted { get; set; }

    public virtual ICollection<DiscountStore> DiscountStores { get; } = new List<DiscountStore>();

    public virtual ICollection<Employee> Employees { get; } = new List<Employee>();

    public virtual ICollection<Order> Orders { get; } = new List<Order>();

    public virtual ICollection<Stock> Stocks { get; } = new List<Stock>();
}
