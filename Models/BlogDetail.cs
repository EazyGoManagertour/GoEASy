using System;
using System.Collections.Generic;

namespace GoEASy.Models;

public partial class BlogDetail
{
    public int BlogDetailId { get; set; }

    public int BlogId { get; set; }

    public string? Introduction { get; set; }

    public string? Section1Title { get; set; }

    public string? Section1Content { get; set; }

    public string? Section2Title { get; set; }

    public string? Section2Content { get; set; }

    public string? Quote { get; set; }

    public string? QuoteAuthor { get; set; }

    public bool? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Blog Blog { get; set; } = null!;
}
