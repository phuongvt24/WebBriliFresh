using System;
using System.Collections.Generic;

namespace WebBriliFresh.Models;

public partial class Feedback
{
    public int FbId { get; set; }

    public int? ReplyId { get; set; }

    public int? ProId { get; set; }

    public int? CusId { get; set; }

    public int? OrderId { get; set; }

    public string? Message { get; set; }

    public DateTime? SendDate { get; set; }

    public int? Rate { get; set; }

    public int? Like { get; set; }

    public virtual Customer? Cus { get; set; }

    public virtual ICollection<FeedbackImage> FeedbackImages { get; } = new List<FeedbackImage>();

    public virtual ICollection<Feedback> InverseReply { get; } = new List<Feedback>();

    public virtual Order? Order { get; set; }

    public virtual Product? Pro { get; set; }

    public virtual Feedback? Reply { get; set; }
}
