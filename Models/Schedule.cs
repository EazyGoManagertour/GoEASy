using System;
using System.Collections.Generic;

namespace GoEASy.Models;

public partial class Schedule
{
    public int ScheduleId { get; set; }

    public int? TourId { get; set; }

    public int DayNumber { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public TimeOnly? StartTime { get; set; }

    public TimeOnly? EndTime { get; set; }

    public string? Location { get; set; }

    public virtual Tour? Tour { get; set; }
}
