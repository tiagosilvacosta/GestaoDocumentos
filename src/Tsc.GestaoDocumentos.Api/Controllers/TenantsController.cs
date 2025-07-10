using Microsoft.AspNetCore.Mvc;
using Tsc.GestaoDocumentos.Application.DTOs;
using Tsc.GestaoDocumentos.Application.DTOs.Common;
using Tsc.GestaoDocumentos.Application.Services;
using Tsc.GestaoDocumentos.Domain.Organizacoes;

namespace Tsc.GestaoDocumentos.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TenantsController : ControllerBase
{
    private readonly ITenantAppService _tenantAppService;

    public TenantsController(ITenantAppService tenantAppService)
    {
        _tenantAppService = tenantAppService;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<TenantDto>>> GetAll([FromQuery] PagedRequest request)
    {
        var result = await _tenantAppService.ObterTodosAsync(request);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TenantDto>> GetById(Guid id)
    {
        var result = await _tenantAppService.ObterPorIdAsync(IdOrganizacao.CriarDeGuid(id));
        
        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpGet("by-slug/{slug}")]
    public async Task<ActionResult<TenantDto>> GetBySlug(string slug)
    {
        var result = await _tenantAppService.ObterPorSlugAsync(slug);
        
        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<TenantDto>> Create([FromBody] CreateTenantDto createTenant)
    {
        try
        {
            var result = await _tenantAppService.CriarAsync(createTenant);
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
            var result = await _tenantAppService.AtualizarAsync(IdOrganizacao.CriarDeGuid(id), updateTenant);
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
        var result = await _tenantAppService.RemoverAsync(IdOrganizacao.CriarDeGuid(id));
        
        if (!result)
            return NotFound();

        return NoContent();
    }
}
