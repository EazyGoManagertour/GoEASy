using System;
using System.Collections.Generic;

namespace GoEASy.Models;

public partial class SemanticQuery
{
    public int ID { get; set; }

    public string Query { get; set; } = null!;

    public int TopK { get; set; }
}
