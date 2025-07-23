using System;
using System.Collections.Generic;

namespace GoEASy.Models;

public partial class Tour
{
    public int TourID { get; set; }

    public string TourName { get; set; } = null!;

    public string? Description { get; set; }

    public decimal AdultPrice { get; set; }

    public decimal ChildPrice { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public int? MaxSeats { get; set; }

    public int? AvailableSeats { get; set; }

    public int? CreatedBy { get; set; }

    public int? DestinationID { get; set; }

    public int? CategoryID { get; set; }

    public bool? Status { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual TourCategory? Category { get; set; }

    public virtual User? CreatedByNavigation { get; set; }

    public virtual Destination? Destination { get; set; }

    public virtual ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();

    public virtual TourDetail? TourDetail { get; set; }

    public virtual ICollection<TourFAQ> TourFAQs { get; set; } = new List<TourFAQ>();

    public virtual ICollection<TourImage> TourImages { get; set; } = new List<TourImage>();

    public virtual ICollection<TourItinerary> TourItineraries { get; set; } = new List<TourItinerary>();
}
