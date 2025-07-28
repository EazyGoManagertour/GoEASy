using System;
using System.Collections.Generic;

namespace GoEASy.Models;

public partial class BlogImage
{
    public int ImageId { get; set; }

    public int BlogId { get; set; }

    public string ImageUrl { get; set; } = null!;

    public int? Position { get; set; }

    public string? Type { get; set; }

    public bool? IsMain { get; set; }

    public DateTime? UploadedAt { get; set; }

    public virtual Blog Blog { get; set; } = null!;
}
