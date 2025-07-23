using System;
using System.Collections.Generic;

namespace GoEASy.Models;

public partial class TourFAQ
{
    public int FAQID { get; set; }

    public int TourID { get; set; }

    public string Question { get; set; } = null!;

    public string? Answer { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Tour Tour { get; set; } = null!;
}
