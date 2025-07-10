using AutoMapper;
using Tsc.GestaoDocumentos.Application.DTOs;
using Tsc.GestaoDocumentos.Application.DTOs.Common;
using Tsc.GestaoDocumentos.Domain.Common;

namespace Tsc.GestaoDocumentos.Application.Services;

/// <summary>
/// Serviço de aplicação para gerenciamento de Donos de Documento.
/// Responsável por orquestrar operações relacionadas a Donos de Documento.
/// </summary>
public class DonoDocumentoAppService : IDonoDocumentoAppService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DonoDocumentoAppService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<DonoDocumentoDto?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var donoDocumento = await _unitOfWork.DonosDocumento.ObterPorIdAsync(id, cancellationToken);
        return donoDocumento != null ? _mapper.Map<DonoDocumentoDto>(donoDocumento) : null;
    }

    public async Task<PagedResult<DonoDocumentoDto>> ObterTodosAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        var donosDocumento = await _unitOfWork.DonosDocumento.ObterTodosAsync(cancellationToken);
        var donosDocumentoDto = _mapper.Map<IEnumerable<DonoDocumentoDto>>(donosDocumento);

        var totalItems = donosDocumentoDto.Count();
        var items = donosDocumentoDto
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize);

        return new PagedResult<DonoDocumentoDto>
        {
            Items = items,
            TotalItems = totalItems,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }

    public Task<DonoDocumentoDto> CriarAsync(CreateDonoDocumentoDto createDonoDocumento, CancellationToken cancellationToken = default)
    {
        // TODO: Implementar lógica de criação
        throw new NotImplementedException("Implementação pendente após criação das entidades de domínio");
    }

    public Task<DonoDocumentoDto> AtualizarAsync(Guid id, UpdateDonoDocumentoDto updateDonoDocumento, CancellationToken cancellationToken = default)
    {
        // TODO: Implementar lógica de atualização
        throw new NotImplementedException("Implementação pendente após criação das entidades de domínio");
    }

    public Task<bool> RemoverAsync(Guid id, CancellationToken cancellationToken = default)
    {
        // TODO: Implementar lógica de remoção
        throw new NotImplementedException("Implementação pendente após criação das entidades de domínio");
    }
}
