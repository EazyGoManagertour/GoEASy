using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GoEASy.Models;

public partial class TourFAQ
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int FaqId { get; set; }

    [Required]
    public int TourId { get; set; }

    [Required]
    [StringLength(255)]
    public string Question { get; set; } = null!;

    public string? Answer { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime CreatedAt { get; set; }

    // Navigation property
    public virtual Tour? Tour { get; set; }
} 