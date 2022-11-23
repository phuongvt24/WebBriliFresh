using System;
using System.Collections.Generic;

namespace WebBriliFresh.Models;

public partial class Employee
{
    public int EmpId { get; set; }

    public int? UserId { get; set; }

    public int? StoreId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public int? Gender { get; set; }

    public string? City { get; set; }

    public string? District { get; set; }

    public string? Ward { get; set; }

    public string? SpecificAddress { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public virtual Store? Store { get; set; }

    public virtual User? User { get; set; }
}
