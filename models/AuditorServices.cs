namespace performance_test.models
{
    public class AuditorServices
    {
        public int Id { get; set; }
        public string ServiceName { get; set; }
        public AuditServiceType? AuditServiceType { get; set; }
        public ICollection<AuditorProfile>? AuditorProfiles { get; set; } = new List<AuditorProfile>();
    }

    public enum AuditServiceType {
        It = 1,
        Finance = 2,
        Operation = 3,
    }
}
