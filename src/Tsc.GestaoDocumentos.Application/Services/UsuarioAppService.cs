using AutoMapper;
using Tsc.GestaoDocumentos.Application.DTOs;
using Tsc.GestaoDocumentos.Application.DTOs.Common;
using Tsc.GestaoDocumentos.Domain.Common;
using Tsc.GestaoDocumentos.Domain.Common.Interfaces;
using Tsc.GestaoDocumentos.Domain.Entities;
using Tsc.GestaoDocumentos.Domain.Enums;
using Tsc.GestaoDocumentos.Domain.Services;

namespace Tsc.GestaoDocumentos.Application.Services;

/// <summary>
/// Serviço de aplicação para gerenciamento de Usuários.
/// Responsável por orquestrar operações relacionadas a Usuários.
/// </summary>
public class UsuarioAppService : IUsuarioAppService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;
    private readonly IAuditoriaService _auditoriaService;

    public UsuarioAppService(
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

    public async Task<UsuarioDto?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var usuario = await _unitOfWork.Usuarios.ObterPorIdAsync(id, cancellationToken);
        return usuario != null ? _mapper.Map<UsuarioDto>(usuario) : null;
    }

    public async Task<PagedResult<UsuarioDto>> ObterTodosAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        var usuarios = await _unitOfWork.Usuarios.ObterTodosAsync(cancellationToken);
        var usuariosDto = _mapper.Map<IEnumerable<UsuarioDto>>(usuarios);

        // TODO: Implementar paginação real no repositório
        var totalItems = usuariosDto.Count();
        var items = usuariosDto
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize);

        return new PagedResult<UsuarioDto>
        {
            Items = items,
            TotalItems = totalItems,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }

    public Task<UsuarioDto> CriarAsync(CreateUsuarioDto createUsuario, CancellationToken cancellationToken = default)
    {
        // TODO: Implementar lógica de criação após finalização das entidades de domínio
        throw new NotImplementedException("Implementação pendente após criação das entidades de domínio completas");
    }

    public Task<UsuarioDto> AtualizarAsync(Guid id, UpdateUsuarioDto updateUsuario, CancellationToken cancellationToken = default)
    {
        // TODO: Implementar lógica de atualização após finalização das entidades de domínio
        throw new NotImplementedException("Implementação pendente após criação das entidades de domínio completas");
    }

    public Task<bool> RemoverAsync(Guid id, CancellationToken cancellationToken = default)
    {
        // TODO: Implementar lógica de remoção após finalização das entidades de domínio
        throw new NotImplementedException("Implementação pendente após criação das entidades de domínio completas");
    }
}
