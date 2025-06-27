using System;
using System.Collections.Generic;

namespace GoEASy.Models;

public partial class Booking
{
    public int BookingId { get; set; }

    public int? UserId { get; set; }

    public int? TourId { get; set; }

<<<<<<< HEAD
    public int? DiscountId { get; set; }

=======
>>>>>>> 33b4a60fd6af242bb2c1562a2cbee06a7b57d2bb
    public DateTime? BookingDate { get; set; }

    public int AdultGuests { get; set; }

    public int ChildGuests { get; set; }

    public string? PaymentStatus { get; set; }

    public int? UsedVippoints { get; set; }

    public decimal? DiscountAmount { get; set; }

    public decimal? TotalPrice { get; set; }

    public bool? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

<<<<<<< HEAD
    public virtual Discount? Discount { get; set; }

=======
>>>>>>> 33b4a60fd6af242bb2c1562a2cbee06a7b57d2bb
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual Tour? Tour { get; set; }

    public virtual User? User { get; set; }
}
