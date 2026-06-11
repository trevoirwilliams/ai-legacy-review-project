using ClaimDesk.Domain.Entities;
using ClaimDesk.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace ClaimDesk.Infrastructure.Persistence;

public static class DbSeeder
{
    public static async Task SeedAsync(ClaimDeskDbContext dbContext, CancellationToken cancellationToken = default)
    {
        await dbContext.Database.EnsureCreatedAsync(cancellationToken);

        if (!await dbContext.Users.AnyAsync(cancellationToken))
        {
            dbContext.Users.AddRange(
                new AppUser { Id = 1, DisplayName = "Morgan Price", Email = "morgan.price@claimdesk.local", Role = "Manager" },
                new AppUser { Id = 2, DisplayName = "Avery Johnson", Email = "avery.johnson@claimdesk.local", Role = "Adjuster" },
                new AppUser { Id = 3, DisplayName = "Riley Chen", Email = "riley.chen@claimdesk.local", Role = "Adjuster" },
                new AppUser { Id = 4, DisplayName = "Casey Brooks", Email = "casey.brooks@claimdesk.local", Role = "CustomerService" });
        }

        if (!await dbContext.InsuranceClaims.AnyAsync(cancellationToken))
        {
            dbContext.InsuranceClaims.AddRange(
                new InsuranceClaim
                {
                    Id = 1,
                    ClaimNumber = "CLM-2026-1001",
                    PolicyNumber = "POL-HOME-44591",
                    CustomerName = "Jordan Lee",
                    CustomerEmail = "jordan.lee@example.com",
                    CustomerPhone = "555-0101",
                    LossDescription = "Kitchen water damage after pipe leak. Customer reports damaged cabinets and flooring.",
                    LossAddress = "42 Palm Avenue",
                    EstimatedLossAmount = 12750.00m,
                    Priority = ClaimPriority.High,
                    Status = ClaimStatus.Assigned,
                    CreatedByUserId = 4,
                    AssignedAdjusterUserId = 2,
                    CreatedAtUtc = DateTime.UtcNow.AddDays(-12)
                },
                new InsuranceClaim
                {
                    Id = 2,
                    ClaimNumber = "CLM-2026-1002",
                    PolicyNumber = "POL-AUTO-11880",
                    CustomerName = "Sam Rivera",
                    CustomerEmail = "sam.rivera@example.com",
                    CustomerPhone = "555-0102",
                    LossDescription = "Rear-end collision. Vehicle not drivable. Customer requested expedited review.",
                    LossAddress = "West Bay Road",
                    EstimatedLossAmount = 8200.00m,
                    Priority = ClaimPriority.Critical,
                    Status = ClaimStatus.InReview,
                    CreatedByUserId = 4,
                    AssignedAdjusterUserId = 3,
                    CreatedAtUtc = DateTime.UtcNow.AddDays(-4)
                });
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
