using System;
using System.Collections.Generic;

namespace GoEASy.Models;

public partial class TourPolicy
{
    public int PolicyID { get; set; }

    public int TourID { get; set; }

    public string PolicyType { get; set; } = null!;

    public string PolicyName { get; set; } = null!;

    public string? PolicyDescription { get; set; }

    public string? PolicyValue { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Tour Tour { get; set; } = null!;
}
