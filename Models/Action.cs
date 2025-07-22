using System;
using System.Collections.Generic;

namespace GoEASy.Models;

public partial class Action
{
    public int ActionId { get; set; }

    public string ActionName { get; set; } = null!;

    public string ActionSlug { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }
}
