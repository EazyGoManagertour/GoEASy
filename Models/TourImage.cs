using System;
using System.Collections.Generic;

namespace GoEASy.Models;

public partial class TourImage
{
    public int ImageID { get; set; }

    public int? TourID { get; set; }

    public string ImageURL { get; set; } = null!;

    public string? Caption { get; set; }

    public bool? IsCover { get; set; }

    public DateTime? UploadedAt { get; set; }

    public virtual Tour? Tour { get; set; }
}
