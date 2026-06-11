using ClaimDesk.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClaimDesk.Infrastructure.Persistence;

public class ClaimDeskDbContext : DbContext
{
    public ClaimDeskDbContext(DbContextOptions<ClaimDeskDbContext> options) : base(options)
    {
    }

    public DbSet<AppUser> Users => Set<AppUser>();
    public DbSet<InsuranceClaim> InsuranceClaims => Set<InsuranceClaim>();
    public DbSet<ClaimActivityNote> ClaimActivityNotes => Set<ClaimActivityNote>();
    public DbSet<ClaimDocument> ClaimDocuments => Set<ClaimDocument>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<InsuranceClaim>(entity =>
        {
            entity.HasIndex(x => x.ClaimNumber).IsUnique();
            entity.Property(x => x.ClaimNumber).HasMaxLength(30);
            entity.Property(x => x.PolicyNumber).HasMaxLength(40);
            entity.Property(x => x.CustomerName).HasMaxLength(150);
            entity.Property(x => x.CustomerEmail).HasMaxLength(200);
            entity.Property(x => x.CustomerPhone).HasMaxLength(50);
            entity.Property(x => x.LossAddress).HasMaxLength(300);
            entity.Property(x => x.EstimatedLossAmount).HasPrecision(18, 2);
        });

        modelBuilder.Entity<ClaimActivityNote>(entity =>
        {
            entity.Property(x => x.NoteType).HasMaxLength(50);
            entity.HasOne(x => x.InsuranceClaim)
                .WithMany(x => x.ActivityNotes)
                .HasForeignKey(x => x.InsuranceClaimId);
        });

        modelBuilder.Entity<ClaimDocument>(entity =>
        {
            entity.Property(x => x.FileName).HasMaxLength(255);
            entity.Property(x => x.ContentType).HasMaxLength(100);
            entity.HasOne(x => x.InsuranceClaim)
                .WithMany(x => x.Documents)
                .HasForeignKey(x => x.InsuranceClaimId);
        });
    }
}
