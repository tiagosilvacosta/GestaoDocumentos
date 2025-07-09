using AutoMapper;
using MediatR;
using Tsc.GestaoDocumentos.Application.Commands.Tenants;
using Tsc.GestaoDocumentos.Application.DTOs;
using Tsc.GestaoDocumentos.Application.Queries.Tenants;
using Tsc.GestaoDocumentos.Domain.Common;
using Tsc.GestaoDocumentos.Domain.Common.Interfaces;
using Tsc.GestaoDocumentos.Domain.Entities;
using Tsc.GestaoDocumentos.Domain.Enums;
using Tsc.GestaoDocumentos.Domain.Services;

namespace Tsc.GestaoDocumentos.Application.Handlers.Tenants;

public class TenantCommandHandler :
    IRequestHandler<CreateTenantCommand, TenantDto>,
    IRequestHandler<UpdateTenantCommand, TenantDto>,
    IRequestHandler<DeleteTenantCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;
    private readonly IAuditoriaService _auditoriaService;

    public TenantCommandHandler(
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

    public async Task<TenantDto> Handle(CreateTenantCommand request, CancellationToken cancellationToken)
    {
        // Verificar se slug já existe
        if (await _unitOfWork.Tenants.SlugExisteAsync(request.Tenant.Slug, cancellationToken))
            throw new InvalidOperationException("Slug já está em uso");

        var tenant = new Tenant(
            request.Tenant.NomeOrganizacao,
            request.Tenant.Slug,
            _currentUserService.UserId);

        if (request.Tenant.DataExpiracao.HasValue)
            tenant.DefinirDataExpiracao(request.Tenant.DataExpiracao, _currentUserService.UserId);

        await _unitOfWork.Tenants.AdicionarAsync(tenant, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _auditoriaService.RegistrarOperacaoAsync(
            tenant.Id,
            _currentUserService.UserId,
            nameof(Tenant),
            tenant.Id,
            TipoOperacaoAuditoria.CREATE,
            "127.0.0.1", // TODO: Obter IP real
            null,
            tenant,
            cancellationToken: cancellationToken);

        return _mapper.Map<TenantDto>(tenant);
    }

    public async Task<TenantDto> Handle(UpdateTenantCommand request, CancellationToken cancellationToken)
    {
        var tenant = await _unitOfWork.Tenants.ObterPorIdAsync(request.Id, cancellationToken);
        if (tenant == null)
            throw new InvalidOperationException("Tenant não encontrado");

        var dadosAnteriores = _mapper.Map<TenantDto>(tenant);

        tenant.DefinirNomeOrganizacao(request.Tenant.NomeOrganizacao);
        
        if (Enum.TryParse<StatusTenant>(request.Tenant.Status, out var status))
            tenant.AlterarStatus(status, _currentUserService.UserId);

        if (request.Tenant.DataExpiracao.HasValue)
            tenant.DefinirDataExpiracao(request.Tenant.DataExpiracao, _currentUserService.UserId);

        await _unitOfWork.Tenants.AtualizarAsync(tenant, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _auditoriaService.RegistrarOperacaoAsync(
            tenant.Id,
            _currentUserService.UserId,
            nameof(Tenant),
            tenant.Id,
            TipoOperacaoAuditoria.UPDATE,
            "127.0.0.1", // TODO: Obter IP real
            dadosAnteriores,
            _mapper.Map<TenantDto>(tenant),
            cancellationToken: cancellationToken);

        return _mapper.Map<TenantDto>(tenant);
    }

    public async Task<bool> Handle(DeleteTenantCommand request, CancellationToken cancellationToken)
    {
        var tenant = await _unitOfWork.Tenants.ObterPorIdAsync(request.Id, cancellationToken);
        if (tenant == null)
            return false;

        var dadosAnteriores = _mapper.Map<TenantDto>(tenant);

        await _unitOfWork.Tenants.RemoverAsync(tenant, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _auditoriaService.RegistrarOperacaoAsync(
            tenant.Id,
            _currentUserService.UserId,
            nameof(Tenant),
            tenant.Id,
            TipoOperacaoAuditoria.DELETE,
            "127.0.0.1", // TODO: Obter IP real
            dadosAnteriores,
            null,
            cancellationToken: cancellationToken);

        return true;
    }
}

public class TenantQueryHandler :
    IRequestHandler<GetTenantByIdQuery, TenantDto?>,
    IRequestHandler<GetTenantBySlugQuery, TenantDto?>,
    IRequestHandler<GetAllTenantsQuery, DTOs.Common.PagedResult<TenantDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public TenantQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<TenantDto?> Handle(GetTenantByIdQuery request, CancellationToken cancellationToken)
    {
        var tenant = await _unitOfWork.Tenants.ObterPorIdAsync(request.Id, cancellationToken);
        return tenant != null ? _mapper.Map<TenantDto>(tenant) : null;
    }

    public async Task<TenantDto?> Handle(GetTenantBySlugQuery request, CancellationToken cancellationToken)
    {
        var tenant = await _unitOfWork.Tenants.ObterPorSlugAsync(request.Slug, cancellationToken);
        return tenant != null ? _mapper.Map<TenantDto>(tenant) : null;
    }

    public async Task<DTOs.Common.PagedResult<TenantDto>> Handle(GetAllTenantsQuery request, CancellationToken cancellationToken)
    {
        var tenants = await _unitOfWork.Tenants.ObterTodosAsync(cancellationToken);
        var tenantsDto = _mapper.Map<IEnumerable<TenantDto>>(tenants);

        // TODO: Implementar paginação real no repositório
        var totalItems = tenantsDto.Count();
        var items = tenantsDto
            .Skip((request.Request.PageNumber - 1) * request.Request.PageSize)
            .Take(request.Request.PageSize);

        return new DTOs.Common.PagedResult<TenantDto>
        {
            Items = items,
            TotalItems = totalItems,
            PageNumber = request.Request.PageNumber,
            PageSize = request.Request.PageSize
        };
    }
}
