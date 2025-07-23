using System;
using System.Collections.Generic;

namespace GoEASy.Models;

public partial class BlogComment
{
    public int CommentID { get; set; }

    public int BlogID { get; set; }

    public int? UserID { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Message { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public virtual Blog Blog { get; set; } = null!;

    public virtual User? User { get; set; }
}
