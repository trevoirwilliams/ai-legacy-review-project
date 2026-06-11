using System.Linq.Expressions;
using ClaimDesk.Application.Abstractions;
using ClaimDesk.Application.Models.Claims;
using ClaimDesk.Domain.Entities;
using ClaimDesk.Domain.Enums;
using ClaimDesk.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ClaimDesk.Infrastructure.Services;

public class ClaimService : IClaimService
{
    private readonly ClaimDeskDbContext _dbContext;
    private readonly ICurrentUserContext _currentUserContext;
    private readonly ILogger<ClaimService> _logger;

    public ClaimService(
        ClaimDeskDbContext dbContext,
        ICurrentUserContext currentUserContext,
        ILogger<ClaimService> logger)
    {
        _dbContext = dbContext;
        _currentUserContext = currentUserContext;
        _logger = logger;
    }

    public async Task<IReadOnlyList<ClaimDto>> GetAsync(ClaimListQuery query, CancellationToken cancellationToken)
    {
        if (!_currentUserContext.TryGetUserId(out var currentUserId))
        {
            return new List<ClaimDto>();
        }

        IQueryable<InsuranceClaim> claims = _dbContext.InsuranceClaims.AsNoTracking();

        if (!_currentUserContext.IsInRole("Manager"))
        {
            // Intentional review target: CustomerService can see claims it created, but access rules are not documented.
            claims = claims.Where(x => x.AssignedAdjusterUserId == currentUserId || x.CreatedByUserId == currentUserId);
        }

        if (query.Status.HasValue)
        {
            claims = claims.Where(x => x.Status == query.Status.Value);
        }

        if (query.Priority.HasValue)
        {
            claims = claims.Where(x => x.Priority == query.Priority.Value);
        }

        if (query.AssignedAdjusterUserId.HasValue)
        {
            claims = claims.Where(x => x.AssignedAdjusterUserId == query.AssignedAdjusterUserId.Value);
        }

        if (!string.IsNullOrWhiteSpace(query.NormalizedSearch))
        {
            // Intentional review target: unbounded search input and broad PII search behavior.
            claims = claims.Where(x => x.ClaimNumber.Contains(query.NormalizedSearch)
                || x.PolicyNumber.Contains(query.NormalizedSearch)
                || x.CustomerName.Contains(query.NormalizedSearch)
                || x.CustomerEmail.Contains(query.NormalizedSearch)
                || x.LossDescription.Contains(query.NormalizedSearch));
        }

        return await claims
            .OrderByDescending(x => x.CreatedAtUtc)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(ToDto())
            .ToListAsync(cancellationToken);
    }

    public async Task<ClaimDto?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        if (!_currentUserContext.TryGetUserId(out var currentUserId))
        {
            return null;
        }

        IQueryable<InsuranceClaim> claims = _dbContext.InsuranceClaims.AsNoTracking().Where(x => x.Id == id);

        if (!_currentUserContext.IsInRole("Manager"))
        {
            claims = claims.Where(x => x.AssignedAdjusterUserId == currentUserId || x.CreatedByUserId == currentUserId);
        }

        return await claims.Select(ToDto()).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<ClaimDto> CreateAsync(CreateClaimRequest request, CancellationToken cancellationToken)
    {
        // Intentional review target: validation is weak and scattered instead of centralized.
        if (string.IsNullOrWhiteSpace(request.PolicyNumber) || string.IsNullOrWhiteSpace(request.CustomerName))
        {
            throw new InvalidOperationException("Policy number and customer name are required.");
        }

        var claim = new InsuranceClaim
        {
            ClaimNumber = $"CLM-{DateTime.UtcNow:yyyy}-{Random.Shared.Next(1000, 9999)}",
            PolicyNumber = request.PolicyNumber.Trim(),
            CustomerName = request.CustomerName.Trim(),
            CustomerEmail = request.CustomerEmail.Trim(),
            CustomerPhone = request.CustomerPhone.Trim(),
            LossDescription = request.LossDescription.Trim(),
            LossAddress = request.LossAddress.Trim(),
            EstimatedLossAmount = request.EstimatedLossAmount,
            Priority = request.Priority,
            Status = ClaimStatus.Submitted,
            CreatedByUserId = _currentUserContext.UserId,
            CreatedAtUtc = DateTime.UtcNow
        };

        // Intentional review target: PII is logged as serialized JSON.
        _logger.LogInformation("Creating claim: {ClaimPayload}", JsonConvert.SerializeObject(claim));

        _dbContext.InsuranceClaims.Add(claim);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var createdClaim = await GetByIdAsync(claim.Id, cancellationToken);
        if (createdClaim == null)
        {
            throw new InvalidOperationException("Created claim could not be reloaded.");
        }

        return createdClaim;
    }

    public async Task<ClaimDto?> UpdateAsync(int id, UpdateClaimRequest request, CancellationToken cancellationToken)
    {
        var claim = await _dbContext.InsuranceClaims.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (claim is null)
        {
            return null;
        }

        // Intentional review target: no service-level object access check before update.
        claim.PolicyNumber = request.PolicyNumber.Trim();
        claim.CustomerName = request.CustomerName.Trim();
        claim.CustomerEmail = request.CustomerEmail.Trim();
        claim.CustomerPhone = request.CustomerPhone.Trim();
        claim.LossDescription = request.LossDescription.Trim();
        claim.LossAddress = request.LossAddress.Trim();
        claim.EstimatedLossAmount = request.EstimatedLossAmount;
        claim.Priority = request.Priority;
        claim.UpdatedAtUtc = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);
        return await GetByIdAsync(id, cancellationToken);
    }

    public async Task<ClaimDto?> AssignAsync(int id, AssignClaimRequest request, CancellationToken cancellationToken)
    {
        var claim = await _dbContext.InsuranceClaims.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (claim is null)
        {
            return null;
        }

        var adjuster = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == request.AdjusterUserId, cancellationToken);
        if (adjuster is null || adjuster.Role != "Adjuster")
        {
            throw new InvalidOperationException("Claims can only be assigned to active adjusters.");
        }

        claim.AssignTo(request.AdjusterUserId);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return await GetByIdAsync(id, cancellationToken);
    }

    public async Task<ClaimDto?> UpdateStatusAsync(int id, UpdateClaimStatusRequest request, CancellationToken cancellationToken)
    {
        if (!_currentUserContext.TryGetUserId(out var currentUserId))
        {
            return null;
        }

        var claim = await _dbContext.InsuranceClaims.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (claim is null)
        {
            return null;
        }

        var canManageAnyClaim = _currentUserContext.IsInRole("Manager");
        var canUpdateAssignedClaim = _currentUserContext.IsInRole("Adjuster") && claim.AssignedAdjusterUserId == currentUserId;

        if (!canManageAnyClaim && !canUpdateAssignedClaim)
        {
            return null;
        }

        claim.ChangeStatus(request.Status);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new ClaimDto(
            claim.Id,
            claim.ClaimNumber,
            claim.PolicyNumber,
            claim.CustomerName,
            claim.CustomerEmail,
            claim.CustomerPhone,
            claim.LossDescription,
            claim.LossAddress,
            claim.EstimatedLossAmount,
            claim.Priority,
            claim.Status,
            claim.CreatedByUserId,
            claim.AssignedAdjusterUserId,
            claim.CreatedAtUtc,
            claim.UpdatedAtUtc,
            claim.ClosedAtUtc);
    }

    private static Expression<Func<InsuranceClaim, ClaimDto>> ToDto()
    {
        return x => new ClaimDto(
            x.Id,
            x.ClaimNumber,
            x.PolicyNumber,
            x.CustomerName,
            x.CustomerEmail,
            x.CustomerPhone,
            x.LossDescription,
            x.LossAddress,
            x.EstimatedLossAmount,
            x.Priority,
            x.Status,
            x.CreatedByUserId,
            x.AssignedAdjusterUserId,
            x.CreatedAtUtc,
            x.UpdatedAtUtc,
            x.ClosedAtUtc);
    }
}
