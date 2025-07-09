using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tsc.GestaoDocumentos.Application.Commands.Tenants;
using Tsc.GestaoDocumentos.Application.DTOs;
using Tsc.GestaoDocumentos.Application.DTOs.Common;
using Tsc.GestaoDocumentos.Application.Queries.Tenants;

namespace Tsc.GestaoDocumentos.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TenantsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TenantsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<TenantDto>>> GetAll([FromQuery] PagedRequest request)
    {
        var result = await _mediator.Send(new GetAllTenantsQuery(request));
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TenantDto>> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetTenantByIdQuery(id));
        
        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpGet("by-slug/{slug}")]
    public async Task<ActionResult<TenantDto>> GetBySlug(string slug)
    {
        var result = await _mediator.Send(new GetTenantBySlugQuery(slug));
        
        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<TenantDto>> Create([FromBody] CreateTenantDto createTenant)
    {
        try
        {
            var result = await _mediator.Send(new CreateTenantCommand(createTenant));
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TenantDto>> Update(Guid id, [FromBody] UpdateTenantDto updateTenant)
    {
        try
        {
            var result = await _mediator.Send(new UpdateTenantCommand(id, updateTenant));
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var result = await _mediator.Send(new DeleteTenantCommand(id));
        
        if (!result)
            return NotFound();

        return NoContent();
    }
}
