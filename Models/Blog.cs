using System;
using System.Collections.Generic;

namespace GoEASy.Models;

public partial class Blog
{
    public int BlogID { get; set; }

    public string Title { get; set; } = null!;

    public string? ShortDescription { get; set; }

    public int CategoryID { get; set; }

    public int? AuthorUserID { get; set; }

    public int? AuthorAdminID { get; set; }

    public byte? IsApproved { get; set; }

    public bool? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Admin? AuthorAdmin { get; set; }

    public virtual User? AuthorUser { get; set; }

    public virtual ICollection<BlogComment> BlogComments { get; set; } = new List<BlogComment>();

    public virtual BlogDetail? BlogDetail { get; set; }

    public virtual ICollection<BlogImage> BlogImages { get; set; } = new List<BlogImage>();

    public virtual ICollection<BlogTagMapping> BlogTagMappings { get; set; } = new List<BlogTagMapping>();

    public virtual TourCategory Category { get; set; } = null!;
}
