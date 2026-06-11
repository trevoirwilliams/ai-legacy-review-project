using ClaimDesk.Domain.Enums;

namespace ClaimDesk.Domain.Entities;

public class InsuranceClaim
{
    private string _customerEmail = string.Empty;
    private string _customerPhone = string.Empty;

    public int Id { get; set; }
    public string ClaimNumber { get; set; } = string.Empty;
    public string PolicyNumber { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;

    // Intentional modernization target: verbose backing fields can be reviewed when modernizing to newer C# versions.
    public string CustomerEmail
    {
        get { return _customerEmail; }
        set { _customerEmail = value == null ? string.Empty : value.Trim(); }
    }

    public string CustomerPhone
    {
        get { return _customerPhone; }
        set { _customerPhone = value == null ? string.Empty : value.Trim(); }
    }

    public string LossDescription { get; set; } = string.Empty;
    public string LossAddress { get; set; } = string.Empty;
    public decimal EstimatedLossAmount { get; set; }
    public ClaimPriority Priority { get; set; } = ClaimPriority.Normal;
    public ClaimStatus Status { get; set; } = ClaimStatus.Draft;
    public int CreatedByUserId { get; set; }
    public int? AssignedAdjusterUserId { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAtUtc { get; set; }
    public DateTime? ClosedAtUtc { get; set; }
    public List<ClaimActivityNote> ActivityNotes { get; set; } = new List<ClaimActivityNote>();
    public List<ClaimDocument> Documents { get; set; } = new List<ClaimDocument>();

    public void AssignTo(int adjusterUserId)
    {
        AssignedAdjusterUserId = adjusterUserId;
        if (Status == ClaimStatus.Submitted)
        {
            Status = ClaimStatus.Assigned;
        }
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void ChangeStatus(ClaimStatus newStatus)
    {
        if (Status == ClaimStatus.Closed)
        {
            throw new InvalidOperationException("Closed claims cannot be changed.");
        }

        // Intentional review target: transition rules are incomplete and scattered.
        if (newStatus == ClaimStatus.Approved || newStatus == ClaimStatus.Denied || newStatus == ClaimStatus.Closed)
        {
            ClosedAtUtc = DateTime.UtcNow;
        }

        Status = newStatus;
        UpdatedAtUtc = DateTime.UtcNow;
    }
}
