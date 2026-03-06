using Microsoft.EntityFrameworkCore;
using performance_test.models;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<AuditNode> AuditNodes => Set<AuditNode>();
    public DbSet<AuditNodeData> AuditNodeData => Set<AuditNodeData>();
    public DbSet<AuditorProfile> AuditorProfiles => Set<AuditorProfile>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Self-referencing tree
        modelBuilder.Entity<AuditNode>()
            .HasOne(n => n.Parent)
            .WithMany(n => n.Children)
            .HasForeignKey(n => n.ParentId)
            .OnDelete(DeleteBehavior.Restrict);

        // AuditNode <-> AuditNodeData (1:1)
        modelBuilder.Entity<AuditNode>()
            .HasOne(n => n.Data)
            .WithOne(d => d.AuditNode)
            .HasForeignKey<AuditNodeData>(d => d.AuditNodeId)
            .OnDelete(DeleteBehavior.Cascade);

        // AuditNode <-> AuditorProfile (1:1 optional)
        modelBuilder.Entity<AuditNode>()
            .HasOne(n => n.Auditor)
            .WithOne(a => a.AuditNode)
            .HasForeignKey<AuditorProfile>(a => a.AuditNodeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}