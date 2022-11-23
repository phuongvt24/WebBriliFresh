using System;
using System.Collections.Generic;

namespace WebBriliFresh.Models;

public partial class QnA
{
    public int QnAid { get; set; }

    public int? ReplyId { get; set; }

    public int? ProId { get; set; }

    public int? CusId { get; set; }

    public string? Message { get; set; }

    public DateTime? SendDate { get; set; }

    public int? Like { get; set; }

    public virtual Customer? Cus { get; set; }

    public virtual ICollection<QnA> InverseReply { get; } = new List<QnA>();

    public virtual Product? Pro { get; set; }

    public virtual ICollection<QnAImage> QnAImages { get; } = new List<QnAImage>();

    public virtual QnA? Reply { get; set; }
}
