using System;
using System.Collections.Generic;

namespace GoEASy.Models;

public partial class BlogTagMapping
{
    public int BlogId { get; set; }

    public int TagId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Blog Blog { get; set; } = null!;

    public virtual BlogTag Tag { get; set; } = null!;
}
