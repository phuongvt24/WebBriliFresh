using System;
using System.Collections.Generic;

namespace WebBriliFresh.Models;

public partial class QnAImage
{
    public int QnAimgId { get; set; }

    public int? QnAid { get; set; }

    public string? ImgData { get; set; }

    public virtual QnA? QnA { get; set; }
}
