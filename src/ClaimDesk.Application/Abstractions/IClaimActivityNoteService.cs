using ClaimDesk.Application.Models.Notes;

namespace ClaimDesk.Application.Abstractions;

public interface IClaimActivityNoteService
{
    Task<IReadOnlyList<ClaimActivityNoteDto>> GetForClaimAsync(int claimId, CancellationToken cancellationToken);
    Task<ClaimActivityNoteDto> AddAsync(int claimId, AddClaimActivityNoteRequest request, CancellationToken cancellationToken);
}
