using System;
using System.Collections.Generic;

namespace GoEASy.Models;

public partial class Rule
{
    public int RuleId { get; set; }

    public string RuleName { get; set; } = null!;

    public string Slug { get; set; } = null!;

    public string? ListRuleSlug { get; set; }

    public bool? IsOpen { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Admin> Admins { get; set; } = new List<Admin>();
}
