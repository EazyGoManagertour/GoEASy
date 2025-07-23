using System;
using System.Collections.Generic;

namespace GoEASy.Models;

public partial class Destination
{
    public int DestinationID { get; set; }

    public string DestinationName { get; set; } = null!;

    public string? Description { get; set; }

    public string? Location { get; set; }

    public bool? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<DestinationImage> DestinationImages { get; set; } = new List<DestinationImage>();

    public virtual ICollection<Tour> Tours { get; set; } = new List<Tour>();
}
