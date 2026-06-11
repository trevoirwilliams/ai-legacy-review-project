using ClaimDesk.Domain.Enums;

namespace ClaimDesk.Application.Models.Claims;

public record ClaimDto(
    int Id,
    string ClaimNumber,
    string PolicyNumber,
    string CustomerName,
    string CustomerEmail,
    string CustomerPhone,
    string LossDescription,
    string LossAddress,
    decimal EstimatedLossAmount,
    ClaimPriority Priority,
    ClaimStatus Status,
    int CreatedByUserId,
    int? AssignedAdjusterUserId,
    DateTime CreatedAtUtc,
    DateTime? UpdatedAtUtc,
    DateTime? ClosedAtUtc);
