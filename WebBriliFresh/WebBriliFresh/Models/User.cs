using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBriliFresh.Models;

public partial class User : IdentityUser<int>
{
    public override int Id { get; set; }

    public override string? UserName { get; set; }

    public string? UserPassword { get; set; }

    public int? UserRole { get; set; }

    [DisplayName("Image Name")]
    public string? Avatar { get; set; } = "download.jfif";

    [NotMapped]
    [DisplayName("Upload File")]
    public IFormFile? ImageFile { get; set; }

    public int? IsDeleted { get; set; } = 0;

    public virtual ICollection<Customer> Customers { get; } = new List<Customer>();

    public virtual ICollection<Employee> Employees { get; } = new List<Employee>();
}
