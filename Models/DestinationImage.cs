using System;
using System.Collections.Generic;

namespace GoEASy.Models;

public partial class DestinationImage
{
    public int ImageId { get; set; }

    public int? DestinationId { get; set; }

    public string ImageUrl { get; set; } = null!;

    public string? Caption { get; set; }

    public bool? IsCover { get; set; }

    public DateTime? UploadedAt { get; set; }

    public virtual Destination? Destination { get; set; }
}
