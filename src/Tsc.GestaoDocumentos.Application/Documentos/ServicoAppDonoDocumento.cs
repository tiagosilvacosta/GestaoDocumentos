using AutoMapper;
using Tsc.GestaoDocumentos.Application.DTOs.Common;
using Tsc.GestaoDocumentos.Domain.Common;
using Tsc.GestaoDocumentos.Domain.Documentos;

namespace Tsc.GestaoDocumentos.Application.Documentos;

/// <summary>
/// Serviço de aplicação para gerenciamento de Donos de Documento.
/// Responsável por orquestrar operações relacionadas a Donos de Documento.
/// </summary>
public class ServicoAppDonoDocumento(IUnitOfWork unitOfWork, IMapper mapper) : IServicoAppDonoDocumento
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<DonoDocumentoDto?> ObterPorIdAsync(IdDonoDocumento id, CancellationToken cancellationToken = default)
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

    public Task<DonoDocumentoDto> AtualizarAsync(IdDonoDocumento id, UpdateDonoDocumentoDto updateDonoDocumento, CancellationToken cancellationToken = default)
    {
        // TODO: Implementar lógica de atualização
        throw new NotImplementedException("Implementação pendente após criação das entidades de domínio");
    }

    public Task<bool> RemoverAsync(IdDonoDocumento id, CancellationToken cancellationToken = default)
    {
        // TODO: Implementar lógica de remoção
        throw new NotImplementedException("Implementação pendente após criação das entidades de domínio");
    }
}
