using System;
using System.Collections.Generic;

namespace GoEASy.Models;

public partial class Favorite
{
    public int UserId { get; set; }

    public int TourId { get; set; }

    public DateTime? AddedDate { get; set; }

    public bool? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Tour Tour { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
