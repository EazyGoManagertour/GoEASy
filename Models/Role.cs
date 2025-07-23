using System;
using System.Collections.Generic;

namespace GoEASy.Models;

public partial class Role
{
    public int RoleID { get; set; }

    public string RoleName { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
