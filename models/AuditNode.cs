using performance_test.models;
using System.ComponentModel.DataAnnotations;

public class AuditNode
{
    public int Id { get; set; }

    // Tree relationship
    public int? ParentId { get; set; }
    public AuditNode? Parent { get; set; }
    public List<AuditNode> Children { get; set; } = new();

    public AuditNodeKind NodeKind { get; set; } = AuditNodeKind.Folder;
    public int SortOrder { get; set; } = 0;

    // 1:1 to NodeData (represents your JSON "data")
    public AuditNodeData Data { get; set; } = default!;

    // Optional 1:1 to Auditor (only when NodeKind == Employee)
    public AuditorProfile? Auditor { get; set; }
}

public enum AuditNodeKind
{
    Folder = 1,
    Employee = 2
}