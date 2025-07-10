using AutoMapper;
using Tsc.GestaoDocumentos.Application.DTOs;
using Tsc.GestaoDocumentos.Application.DTOs.Common;
using Tsc.GestaoDocumentos.Domain.Common;
using Tsc.GestaoDocumentos.Domain.Logs;
using Tsc.GestaoDocumentos.Domain.Organizacoes;

namespace Tsc.GestaoDocumentos.Application.Services;

/// <summary>
/// Serviço de aplicação para gerenciamento de Tenants.
/// Responsável por orquestrar operações relacionadas a Tenants.
/// </summary>
public class TenantAppService : ITenantAppService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;
    private readonly IAuditoriaService _auditoriaService;

    public TenantAppService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ICurrentUserService currentUserService,
        IAuditoriaService auditoriaService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
        _auditoriaService = auditoriaService;
    }

    public async Task<TenantDto?> ObterPorIdAsync(IdOrganizacao id, CancellationToken cancellationToken = default)
    {
        var tenant = await _unitOfWork.Tenants.ObterPorIdAsync(id, cancellationToken);
        return tenant != null ? _mapper.Map<TenantDto>(tenant) : null;
    }

    public async Task<TenantDto?> ObterPorSlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        var tenant = await _unitOfWork.Tenants.ObterPorSlugAsync(slug, cancellationToken);
        return tenant != null ? _mapper.Map<TenantDto>(tenant) : null;
    }

    public async Task<PagedResult<TenantDto>> ObterTodosAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        var tenants = await _unitOfWork.Tenants.ObterTodosAsync(cancellationToken);
        var tenantsDto = _mapper.Map<IEnumerable<TenantDto>>(tenants);

        // TODO: Implementar paginação real no repositório
        var totalItems = tenantsDto.Count();
        var items = tenantsDto
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize);

        return new PagedResult<TenantDto>
        {
            Items = items,
            TotalItems = totalItems,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }

    public async Task<TenantDto> CriarAsync(CreateTenantDto createTenant, CancellationToken cancellationToken = default)
    {
        // Verificar se slug já existe
        if (await _unitOfWork.Tenants.SlugExisteAsync(createTenant.Slug, cancellationToken))
            throw new InvalidOperationException("Slug já está em uso");

        var organizacao = new Organizacao(
            createTenant.NomeOrganizacao,
            createTenant.Slug,
            _currentUserService.IdUsuario);

        if (createTenant.DataExpiracao.HasValue)
            organizacao.DefinirDataExpiracao(createTenant.DataExpiracao, _currentUserService.IdUsuario);

        await _unitOfWork.Tenants.AdicionarAsync(organizacao, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _auditoriaService.RegistrarOperacaoAsync(
            organizacao.Id,
            _currentUserService.IdUsuario,
            nameof(Organizacao),
            organizacao.Id.Valor,
            TipoOperacaoAuditoria.CREATE,
            "127.0.0.1", // TODO: Obter IP real
            null,
            organizacao,
            cancellationToken: cancellationToken);

        return _mapper.Map<TenantDto>(organizacao);
    }

    public async Task<TenantDto> AtualizarAsync(IdOrganizacao id, UpdateTenantDto updateTenant, CancellationToken cancellationToken = default)
    {
        var tenant = await _unitOfWork.Tenants.ObterPorIdAsync(id, cancellationToken);
        if (tenant == null)
            throw new InvalidOperationException("Tenant não encontrado");

        var dadosAnteriores = _mapper.Map<TenantDto>(tenant);

        tenant.DefinirNomeOrganizacao(updateTenant.NomeOrganizacao);
        
        if (Enum.TryParse<StatusTenant>(updateTenant.Status, out var status))
            tenant.AlterarStatus(status, _currentUserService.IdUsuario);

        if (updateTenant.DataExpiracao.HasValue)
            tenant.DefinirDataExpiracao(updateTenant.DataExpiracao, _currentUserService.IdUsuario);

        await _unitOfWork.Tenants.AtualizarAsync(tenant, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _auditoriaService.RegistrarOperacaoAsync(
            tenant.Id,
            _currentUserService.IdUsuario,
            nameof(Organizacao),
            tenant.Id.Valor,
            TipoOperacaoAuditoria.UPDATE,
            "127.0.0.1", // TODO: Obter IP real
            dadosAnteriores,
            _mapper.Map<TenantDto>(tenant),
            cancellationToken: cancellationToken);

        return _mapper.Map<TenantDto>(tenant);
    }

    public async Task<bool> RemoverAsync(IdOrganizacao id, CancellationToken cancellationToken = default)
    {
        var tenant = await _unitOfWork.Tenants.ObterPorIdAsync(id, cancellationToken);
        if (tenant == null)
            return false;

        var dadosAnteriores = _mapper.Map<TenantDto>(tenant);

        await _unitOfWork.Tenants.RemoverAsync(tenant, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _auditoriaService.RegistrarOperacaoAsync(
            tenant.Id,
            _currentUserService.IdUsuario,
            nameof(Organizacao),
            tenant.Id.Valor,
            TipoOperacaoAuditoria.DELETE,
            "127.0.0.1", // TODO: Obter IP real
            dadosAnteriores,
            null,
            cancellationToken: cancellationToken);

        return true;
    }
}
