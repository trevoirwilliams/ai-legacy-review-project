namespace ClaimDesk.Domain.Entities;

public class ClaimDocument
{
    public int Id { get; set; }
    public int InsuranceClaimId { get; set; }
    public InsuranceClaim? InsuranceClaim { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public string LocalPath { get; set; } = string.Empty;
    public long SizeInBytes { get; set; }
    public int UploadedByUserId { get; set; }
    public DateTime UploadedAtUtc { get; set; } = DateTime.UtcNow;
}
