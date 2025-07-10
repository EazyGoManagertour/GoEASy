using System;
using System.Collections.Generic;

namespace GoEASy.Models;

public partial class BlogTag
{
    public int TagId { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Blog> Blogs { get; set; } = new List<Blog>();
}
