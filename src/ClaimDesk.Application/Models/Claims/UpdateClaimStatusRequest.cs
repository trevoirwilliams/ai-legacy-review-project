using ClaimDesk.Domain.Enums;

namespace ClaimDesk.Application.Models.Claims;

public class UpdateClaimStatusRequest
{
    public ClaimStatus Status { get; set; }
}
