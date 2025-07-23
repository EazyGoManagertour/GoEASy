using System;
using System.Collections.Generic;

namespace GoEASy.Models;

public partial class Review
{
    public int ReviewID { get; set; }

    public int? UserID { get; set; }

    public int? TourID { get; set; }

    public int? Rating { get; set; }

    public string? Comment { get; set; }

    public string? SentimentScore { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual Tour? Tour { get; set; }

    public virtual User? User { get; set; }
}
