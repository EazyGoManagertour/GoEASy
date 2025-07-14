using System;
using System.Collections.Generic;

namespace GoEASy.Models;

public partial class Blog
{
    public int BlogId { get; set; }

    public string Title { get; set; } = null!;

    public string? ShortDescription { get; set; }

    public int CategoryId { get; set; }

    public int? AuthorUserId { get; set; }

    public int? AuthorAdminId { get; set; }

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
