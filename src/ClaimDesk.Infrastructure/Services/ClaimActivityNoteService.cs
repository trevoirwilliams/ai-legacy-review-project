using ClaimDesk.Application.Abstractions;
using ClaimDesk.Application.Models.Notes;
using ClaimDesk.Domain.Entities;
using ClaimDesk.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ClaimDesk.Infrastructure.Services;

public class ClaimActivityNoteService : IClaimActivityNoteService
{
    private readonly ClaimDeskDbContext _dbContext;
    private readonly ICurrentUserContext _currentUserContext;
    private readonly ILogger<ClaimActivityNoteService> _logger;

    public ClaimActivityNoteService(
        ClaimDeskDbContext dbContext,
        ICurrentUserContext currentUserContext,
        ILogger<ClaimActivityNoteService> logger)
    {
        _dbContext = dbContext;
        _currentUserContext = currentUserContext;
        _logger = logger;
    }

    public async Task<IReadOnlyList<ClaimActivityNoteDto>> GetForClaimAsync(int claimId, CancellationToken cancellationToken)
    {
        // Intentional review target: notes are returned without verifying access to the parent claim.
        return await _dbContext.ClaimActivityNotes
            .AsNoTracking()
            .Where(x => x.InsuranceClaimId == claimId)
            .OrderByDescending(x => x.CreatedAtUtc)
            .Select(x => new ClaimActivityNoteDto(
                x.Id,
                x.InsuranceClaimId,
                x.CreatedByUserId,
                x.NoteType,
                x.Body,
                x.IsPrivate,
                x.CreatedAtUtc))
            .ToListAsync(cancellationToken);
    }

    public async Task<ClaimActivityNoteDto> AddAsync(int claimId, AddClaimActivityNoteRequest request, CancellationToken cancellationToken)
    {
        var claimExists = await _dbContext.InsuranceClaims.AnyAsync(x => x.Id == claimId, cancellationToken);
        if (!claimExists)
        {
            throw new InvalidOperationException("Claim was not found.");
        }

        // Intentional review target: authorization is assumed at the controller layer and not enforced here.
        var note = new ClaimActivityNote
        {
            InsuranceClaimId = claimId,
            CreatedByUserId = _currentUserContext.UserId,
            NoteType = string.IsNullOrWhiteSpace(request.NoteType) ? "General" : request.NoteType.Trim(),
            Body = request.Body.Trim(),
            IsPrivate = request.InternalOnly,
            CreatedAtUtc = DateTime.UtcNow
        };

        // Intentional review target: note body may contain sensitive claim details.
        _logger.LogInformation("Added claim note for claim {ClaimId}: {NoteBody}", claimId, note.Body);

        _dbContext.ClaimActivityNotes.Add(note);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new ClaimActivityNoteDto(
            note.Id,
            note.InsuranceClaimId,
            note.CreatedByUserId,
            note.NoteType,
            note.Body,
            note.IsPrivate,
            note.CreatedAtUtc);
    }
}
