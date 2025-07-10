using AutoMapper;
using Tsc.GestaoDocumentos.Application.DTOs.Common;
using Tsc.GestaoDocumentos.Domain.Common;
using Tsc.GestaoDocumentos.Domain.Documentos;

namespace Tsc.GestaoDocumentos.Application.Documentos;

/// <summary>
/// Serviço de aplicação para gerenciamento de Tipos de Documento.
/// Responsável por orquestrar operações relacionadas a Tipos de Documento.
/// </summary>
public class ServicoAppTipoDocumento(IUnitOfWork unitOfWork, IMapper mapper) : IServicoAppTipoDocumento
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<TipoDocumentoDto?> ObterPorIdAsync(IdTipoDocumento id, CancellationToken cancellationToken = default)
    {
        var tipoDocumento = await _unitOfWork.TiposDocumento.ObterPorIdAsync(id, cancellationToken);
        return tipoDocumento != null ? _mapper.Map<TipoDocumentoDto>(tipoDocumento) : null;
    }

    public async Task<PagedResult<TipoDocumentoDto>> ObterTodosAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        var tiposDocumento = await _unitOfWork.TiposDocumento.ObterTodosAsync(cancellationToken);
        var tiposDocumentoDto = _mapper.Map<IEnumerable<TipoDocumentoDto>>(tiposDocumento);

        var totalItems = tiposDocumentoDto.Count();
        var items = tiposDocumentoDto
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize);

        return new PagedResult<TipoDocumentoDto>
        {
            Items = items,
            TotalItems = totalItems,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }

    public Task<TipoDocumentoDto> CriarAsync(CreateTipoDocumentoDto createTipoDocumento, CancellationToken cancellationToken = default)
    {
        // TODO: Implementar lógica de criação
        throw new NotImplementedException("Implementação pendente após criação das entidades de domínio");
    }

    public Task<TipoDocumentoDto> AtualizarAsync(IdTipoDocumento id, UpdateTipoDocumentoDto updateTipoDocumento, CancellationToken cancellationToken = default)
    {
        // TODO: Implementar lógica de atualização
        throw new NotImplementedException("Implementação pendente após criação das entidades de domínio");
    }

    public Task<TipoDocumentoDto> VincularTiposDonoAsync(IdTipoDocumento id, VincularTipoDonoDto vincularTipoDono, CancellationToken cancellationToken = default)
    {
        // TODO: Implementar lógica de vinculação
        throw new NotImplementedException("Implementação pendente após criação das entidades de domínio");
    }

    public Task<bool> RemoverAsync(IdTipoDocumento id, CancellationToken cancellationToken = default)
    {
        // TODO: Implementar lógica de remoção
        throw new NotImplementedException("Implementação pendente após criação das entidades de domínio");
    }
}
