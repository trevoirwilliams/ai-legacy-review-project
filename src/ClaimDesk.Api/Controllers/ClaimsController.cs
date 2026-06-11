using ClaimDesk.Application.Abstractions;
using ClaimDesk.Application.Models.Claims;
using Microsoft.AspNetCore.Mvc;

namespace ClaimDesk.Api.Controllers;

[ApiController]
[Route("api/claims")]
public class ClaimsController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get(
        [FromServices] IClaimService service,
        [FromQuery] ClaimListQuery query,
        CancellationToken cancellationToken)
    {
        var claims = await service.GetAsync(query, cancellationToken);
        return Ok(claims);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(
        int id,
        [FromServices] IClaimService service,
        CancellationToken cancellationToken)
    {
        var claim = await service.GetByIdAsync(id, cancellationToken);
        return claim is null ? NotFound() : Ok(claim);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromServices] IClaimService service,
        [FromBody] CreateClaimRequest request,
        CancellationToken cancellationToken)
    {
        var created = await service.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        [FromServices] IClaimService service,
        [FromBody] UpdateClaimRequest request,
        CancellationToken cancellationToken)
    {
        var updated = await service.UpdateAsync(id, request, cancellationToken);
        return updated is null ? NotFound() : Ok(updated);
    }

    [HttpPatch("{id:int}/assign")]
    public async Task<IActionResult> Assign(
        int id,
        [FromServices] IClaimService service,
        [FromBody] AssignClaimRequest request,
        CancellationToken cancellationToken)
    {
        var updated = await service.AssignAsync(id, request, cancellationToken);
        return updated is null ? NotFound() : Ok(updated);
    }

    [HttpPatch("{id:int}/status")]
    public async Task<IActionResult> UpdateStatus(
        int id,
        [FromServices] IClaimService service,
        [FromBody] UpdateClaimStatusRequest request,
        CancellationToken cancellationToken)
    {
        var updated = await service.UpdateStatusAsync(id, request, cancellationToken);
        return updated is null ? NotFound() : Ok(updated);
    }
}
