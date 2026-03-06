using System.ComponentModel.DataAnnotations;

public class AuditorProfile
{
    public int Id { get; set; } // PK

    [MaxLength(50)]
    public string? EmpId { get; set; }

    [MaxLength(100)]
    public string? Dept { get; set; }

    [MaxLength(200)]
    public string? Contact { get; set; }

    [MaxLength(500)]
    public string? Image { get; set; }

    [MaxLength(2000)]
    public string? Description { get; set; }

    // FK (1:1) to AuditNode
    public int AuditNodeId { get; set; }
    public AuditNode AuditNode { get; set; } = default!;
}