using System;
using System.Collections.Generic;

namespace GoEASy.Models;

public partial class TourFaq
{
    public int Faqid { get; set; }

    public int TourId { get; set; }

    public string Question { get; set; } = null!;

    public string? Answer { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Tour Tour { get; set; } = null!;
}
