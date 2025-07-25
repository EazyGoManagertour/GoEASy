using System;
using System.Collections.Generic;

namespace GoEASy.Models;

public partial class TourViewLog
{
    public int ID { get; set; }

    public int? TourID { get; set; }

    public int? UserID { get; set; }

    public DateTime ViewTime { get; set; }

    public string ActionType { get; set; } = null!;
}
