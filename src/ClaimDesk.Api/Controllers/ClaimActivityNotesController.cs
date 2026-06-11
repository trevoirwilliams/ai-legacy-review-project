using ClaimDesk.Application.Abstractions;
using ClaimDesk.Application.Models.Notes;
using Microsoft.AspNetCore.Mvc;

namespace ClaimDesk.Api.Controllers;

[ApiController]
[Route("api/claims/{claimId:int}/activity-notes")]
public class ClaimActivityNotesController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetForClaim(
        int claimId,
        [FromServices] IClaimActivityNoteService service,
        CancellationToken cancellationToken)
    {
        var notes = await service.GetForClaimAsync(claimId, cancellationToken);
        return Ok(notes);
    }

    [HttpPost]
    public async Task<IActionResult> Add(
        int claimId,
        [FromServices] IClaimActivityNoteService service,
        [FromBody] AddClaimActivityNoteRequest request,
        CancellationToken cancellationToken)
    {
        var created = await service.AddAsync(claimId, request, cancellationToken);
        return CreatedAtAction(nameof(GetForClaim), new { claimId }, created);
    }
}
