using System;
using System.Collections.Generic;

namespace GoEASy.Models;

public partial class Discount
{
    public int DiscountId { get; set; }

    public string Code { get; set; } = null!;

    public string? Description { get; set; }

    public int? Percentage { get; set; }

    public decimal? MaxAmount { get; set; }

    public decimal? MinTotalPrice { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public bool? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
