using System;
using System.Collections.Generic;

namespace GoEASy.Models;

public partial class TourDetail
{
    public int TourDetailID { get; set; }

    public int TourID { get; set; }

    public string? Description { get; set; }

    public string? Included { get; set; }

    public string? Excluded { get; set; }

    public string? Activities { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Tour Tour { get; set; } = null!;
}
