using AutoMapper;
using Tsc.GestaoDocumentos.Application.DTOs;
using Tsc.GestaoDocumentos.Application.DTOs.Common;
using Tsc.GestaoDocumentos.Domain.Common;

namespace Tsc.GestaoDocumentos.Application.Services;

/// <summary>
/// Serviço de aplicação para gerenciamento de Tipos de Dono.
/// Responsável por orquestrar operações relacionadas a Tipos de Dono.
/// </summary>
public class TipoDonoAppService : ITipoDonoAppService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public TipoDonoAppService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<TipoDonoDto?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var tipoDono = await _unitOfWork.TiposDono.ObterPorIdAsync(id, cancellationToken);
        return tipoDono != null ? _mapper.Map<TipoDonoDto>(tipoDono) : null;
    }

    public async Task<PagedResult<TipoDonoDto>> ObterTodosAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        var tiposDono = await _unitOfWork.TiposDono.ObterTodosAsync(cancellationToken);
        var tiposDonoDto = _mapper.Map<IEnumerable<TipoDonoDto>>(tiposDono);

        var totalItems = tiposDonoDto.Count();
        var items = tiposDonoDto
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize);

        return new PagedResult<TipoDonoDto>
        {
            Items = items,
            TotalItems = totalItems,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }

    public Task<TipoDonoDto> CriarAsync(CreateTipoDonoDto createTipoDono, CancellationToken cancellationToken = default)
    {
        // TODO: Implementar lógica de criação
        throw new NotImplementedException("Implementação pendente após criação das entidades de domínio");
    }

    public Task<TipoDonoDto> AtualizarAsync(Guid id, UpdateTipoDonoDto updateTipoDono, CancellationToken cancellationToken = default)
    {
        // TODO: Implementar lógica de atualização
        throw new NotImplementedException("Implementação pendente após criação das entidades de domínio");
    }

    public Task<TipoDonoDto> VincularTiposDocumentoAsync(Guid id, VincularTipoDocumentoDto vincularTipoDocumento, CancellationToken cancellationToken = default)
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
