using System;
using System.Collections.Generic;

namespace WebBriliFresh.Models;

public partial class FeedbackImage
{
    public int FbImgId { get; set; }

    public string? ImgData { get; set; }

    public int? FbId { get; set; }

    public virtual Feedback? Fb { get; set; }
}
