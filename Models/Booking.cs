using System;
using System.Collections.Generic;

namespace GoEASy.Models;

public partial class Booking
{
    public int BookingID { get; set; }

    public int? UserID { get; set; }

    public int? TourID { get; set; }

    public int? DiscountID { get; set; }

    public DateTime? BookingDate { get; set; }

    public int AdultGuests { get; set; }

    public int ChildGuests { get; set; }

    public string? PaymentStatus { get; set; }

    public int? UsedVIPPoints { get; set; }

    public decimal? DiscountAmount { get; set; }

    public decimal? TotalPrice { get; set; }

    public bool? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Discount? Discount { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual Tour? Tour { get; set; }

    public virtual User? User { get; set; }
}
