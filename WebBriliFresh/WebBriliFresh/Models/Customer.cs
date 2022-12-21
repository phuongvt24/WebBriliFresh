using System;
using System.Collections.Generic;

namespace WebBriliFresh.Models;

public partial class Customer
{
    public int CusId { get; set; }

    public int? UserId { get; set; }

    public int? RewardId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public int? Gender { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public virtual ICollection<Address> Addresses { get; } = new List<Address>();

    public virtual ICollection<Feedback> Feedbacks { get; } = new List<Feedback>();

    public virtual ICollection<QnA> QnAs { get; } = new List<QnA>();

    public virtual ICollection<Order> Orders { get; } = new List<Order>();

    public virtual Reward? Reward { get; set; }

    public virtual User? User { get; set; }
}
