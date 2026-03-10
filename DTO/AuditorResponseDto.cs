using performance_test.models;

namespace performance_test.DTO
{
    public class AuditorResponseDto
    {
        public int Id { get; set; }
        public string? EmpId { get; set; }
        public string? Dept { get; set; }
        public string? Contact { get; set; }
        public string? Image { get; set; }
        public string? Description { get; set; }
        public ICollection<AuditServiceDto> Services { get; set; } = new List<AuditServiceDto>();
        public SimpleAuditNodeDto? AuditNode { get; set; }
    }
}
