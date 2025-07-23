using System;
using System.Collections.Generic;

namespace GoEASy.Models;

public partial class Payment
{
    public int PaymentID { get; set; }

    public int? BookingID { get; set; }

    public decimal Amount { get; set; }

    public string? PaymentMethod { get; set; }

    public DateTime? PaymentDate { get; set; }

    public string? Status { get; set; }

    public virtual Booking? Booking { get; set; }
}
