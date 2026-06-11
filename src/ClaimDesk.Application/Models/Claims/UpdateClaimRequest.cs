using ClaimDesk.Domain.Enums;

namespace ClaimDesk.Application.Models.Claims;

public class UpdateClaimRequest
{
    public string PolicyNumber { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public string CustomerPhone { get; set; } = string.Empty;
    public string LossDescription { get; set; } = string.Empty;
    public string LossAddress { get; set; } = string.Empty;
    public decimal EstimatedLossAmount { get; set; }
    public ClaimPriority Priority { get; set; } = ClaimPriority.Normal;
}
