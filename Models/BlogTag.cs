using System;
using System.Collections.Generic;

namespace GoEASy.Models;

public partial class BlogTag
{
    public int TagId { get; set; }

    public string? Name { get; set; }

    public bool? Status { get; set; }

    public virtual ICollection<BlogTagMapping> BlogTagMappings { get; set; } = new List<BlogTagMapping>();
}
