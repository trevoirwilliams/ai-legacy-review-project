namespace ClaimDesk.Application.Abstractions;

public interface ICurrentUserContext
{
    int UserId { get; }
    string Role { get; }
    bool TryGetUserId(out int userId);
    bool IsInRole(string role);
}
