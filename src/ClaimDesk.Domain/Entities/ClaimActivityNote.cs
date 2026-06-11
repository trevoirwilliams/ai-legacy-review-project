namespace ClaimDesk.Domain.Entities;

public class ClaimActivityNote
{
    public int Id { get; set; }
    public int InsuranceClaimId { get; set; }
    public InsuranceClaim? InsuranceClaim { get; set; }
    public int CreatedByUserId { get; set; }
    public string NoteType { get; set; } = "General";
    public string Body { get; set; } = string.Empty;
    public bool IsPrivate { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}
