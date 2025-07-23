using System;
using System.Collections.Generic;

namespace GoEASy.Models;

public partial class Companion
{
    public int CompanionID { get; set; }

    public int UserID { get; set; }

    public string FullName { get; set; } = null!;

    public string? Gender { get; set; }

    public DateOnly? BirthDate { get; set; }

    public string? NationalID { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual User User { get; set; } = null!;
}
