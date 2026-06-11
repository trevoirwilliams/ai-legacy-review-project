using ClaimDesk.Domain.Enums;

namespace ClaimDesk.Application.Models.Claims;

public class ClaimListQuery
{
    public ClaimStatus? Status { get; set; }
    public ClaimPriority? Priority { get; set; }
    public int? AssignedAdjusterUserId { get; set; }
    public string? Search { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 100;

    public string? NormalizedSearch => string.IsNullOrWhiteSpace(Search) ? null : Search.Trim();
}
