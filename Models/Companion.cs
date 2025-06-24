using System;
using System.Collections.Generic;

namespace GoEASy.Models;

public partial class Companion
{
    public int CompanionId { get; set; }

    public int UserId { get; set; }

    public string FullName { get; set; } = null!;

    public string? Gender { get; set; }

    public DateOnly? BirthDate { get; set; }

    public string? NationalId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual User User { get; set; } = null!;
}
