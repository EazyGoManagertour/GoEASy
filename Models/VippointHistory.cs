using System;
using System.Collections.Generic;

namespace GoEASy.Models;

public partial class VippointHistory
{
    public int HistoryId { get; set; }

    public int? UserId { get; set; }

    public int? ChangeAmount { get; set; }

    public string? Reason { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual User? User { get; set; }
}
