using System;
using System.Collections.Generic;

namespace WebBriliFresh.Models;

public partial class ProductImage
{
    public int ImgId { get; set; }

    public int? ProId { get; set; }

    public string? ImgData { get; set; }

    public virtual Product? Pro { get; set; }
}
