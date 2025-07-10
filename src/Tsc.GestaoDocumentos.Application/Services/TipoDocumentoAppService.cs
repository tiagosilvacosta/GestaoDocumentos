using AutoMapper;
using Tsc.GestaoDocumentos.Application.DTOs;
using Tsc.GestaoDocumentos.Application.DTOs.Common;
using Tsc.GestaoDocumentos.Domain.Common;

namespace Tsc.GestaoDocumentos.Application.Services;

/// <summary>
/// Serviço de aplicação para gerenciamento de Tipos de Documento.
/// Responsável por orquestrar operações relacionadas a Tipos de Documento.
/// </summary>
public class TipoDocumentoAppService : ITipoDocumentoAppService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public TipoDocumentoAppService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<TipoDocumentoDto?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default)
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

    public Task<TipoDocumentoDto> AtualizarAsync(Guid id, UpdateTipoDocumentoDto updateTipoDocumento, CancellationToken cancellationToken = default)
    {
        // TODO: Implementar lógica de atualização
        throw new NotImplementedException("Implementação pendente após criação das entidades de domínio");
    }

    public Task<TipoDocumentoDto> VincularTiposDonoAsync(Guid id, VincularTipoDonoDto vincularTipoDono, CancellationToken cancellationToken = default)
    {
        // TODO: Implementar lógica de vinculação
        throw new NotImplementedException("Implementação pendente após criação das entidades de domínio");
    }

    public Task<bool> RemoverAsync(Guid id, CancellationToken cancellationToken = default)
    {
        // TODO: Implementar lógica de remoção
        throw new NotImplementedException("Implementação pendente após criação das entidades de domínio");
    }
}
