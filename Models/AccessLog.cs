using System;
using System.Collections.Generic;

namespace GoEASy.Models;

public partial class AccessLog
{
    public int LogID { get; set; }

    public int? UserID { get; set; }

    public string? IPAddress { get; set; }

    public string? UserAgent { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual User? User { get; set; }
}
