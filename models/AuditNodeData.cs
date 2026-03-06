using System.ComponentModel.DataAnnotations;

namespace performance_test.models
{
    public class AuditNodeData
    {
        public int Id { get; set; } // PK (can also be AuditNodeId, see fluent config)

        [Required, MaxLength(200)]
        public string Name { get; set; } = default!;

        [MaxLength(50)]
        public string? Type { get; set; }   // IT/Finance/Operations OR role

        [MaxLength(50)]
        public string? Size { get; set; }   // "12 staff" etc.

        // FK (1:1) to AuditNode
        public int AuditNodeId { get; set; }
        public AuditNode AuditNode { get; set; } = default!;
    }
}
