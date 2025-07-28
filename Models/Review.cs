using System;
using System.Collections.Generic;

namespace GoEASy.Models;

public partial class Review
{
    public int ReviewId { get; set; }

    public int? UserId { get; set; }

    public int? TourId { get; set; }

    public int? Rating { get; set; }

    public string? Comment { get; set; }

    public string? SentimentScore { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual Tour? Tour { get; set; }

    public virtual User? User { get; set; }
}
