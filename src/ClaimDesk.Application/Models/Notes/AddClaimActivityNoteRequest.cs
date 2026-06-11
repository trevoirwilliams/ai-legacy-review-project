namespace ClaimDesk.Application.Models.Notes;

public class AddClaimActivityNoteRequest
{
    public string NoteType { get; set; } = "General";
    public string Body { get; set; } = string.Empty;
    public bool InternalOnly { get; set; }
}
