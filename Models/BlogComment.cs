using System;
using System.Collections.Generic;

namespace GoEASy.Models;

public partial class BlogComment
{
    public int CommentId { get; set; }

    public int BlogId { get; set; }

    public int? UserId { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Message { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public virtual Blog Blog { get; set; } = null!;

    public virtual User? User { get; set; }
}
