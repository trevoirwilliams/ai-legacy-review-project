using ClaimDesk.Application.Models.Claims;

namespace ClaimDesk.Application.Abstractions;

public interface IClaimService
{
    Task<IReadOnlyList<ClaimDto>> GetAsync(ClaimListQuery query, CancellationToken cancellationToken);
    Task<ClaimDto?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<ClaimDto> CreateAsync(CreateClaimRequest request, CancellationToken cancellationToken);
    Task<ClaimDto?> UpdateAsync(int id, UpdateClaimRequest request, CancellationToken cancellationToken);
    Task<ClaimDto?> AssignAsync(int id, AssignClaimRequest request, CancellationToken cancellationToken);
    Task<ClaimDto?> UpdateStatusAsync(int id, UpdateClaimStatusRequest request, CancellationToken cancellationToken);
}
