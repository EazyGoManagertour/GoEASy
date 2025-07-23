﻿using System;
using System.Collections.Generic;

namespace GoEASy.Models;

public partial class Notification
{
    public int NotificationId { get; set; }

    public int UserId { get; set; }

    public string Message { get; set; } = null!;

    public string? Title { get; set; }

    public bool? IsRead { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? Type { get; set; }

    public int? RelatedId { get; set; }

    public virtual User User { get; set; } = null!;
}
