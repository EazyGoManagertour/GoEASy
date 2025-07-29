using System;
using System.Collections.Generic;

namespace GoEASy.Models;

public partial class Contact
{
    public int ContactID { get; set; }

    public int? UserID { get; set; }

    public string FullName { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Message { get; set; } = null!;

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual User? User { get; set; }
}
