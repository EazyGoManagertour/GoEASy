using System;
using System.Collections.Generic;

namespace GoEASy.Models;

public partial class TourItinerary
{
    public int ItineraryID { get; set; }

    public int? TourID { get; set; }

    public int? DayNumber { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? Meals { get; set; }

    public string? Accommodation { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Tour? Tour { get; set; }
}
