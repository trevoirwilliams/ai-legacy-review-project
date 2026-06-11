namespace ClaimDesk.Application.Models.Notes;

public record ClaimActivityNoteDto(
    int Id,
    int InsuranceClaimId,
    int CreatedByUserId,
    string NoteType,
    string Body,
    bool IsPrivate,
    DateTime CreatedAtUtc);
