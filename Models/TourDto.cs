using System;
using System.Collections.Generic;

namespace GoEASy.Models;

public partial class TourDto
{
    public int ID { get; set; }

    public string Title { get; set; } = null!;

    public string? Desc { get; set; }

    public decimal Price { get; set; }
}
