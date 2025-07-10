using AutoMapper;
using Tsc.GestaoDocumentos.Application.DTOs.Common;
using Tsc.GestaoDocumentos.Domain.Common;
using Tsc.GestaoDocumentos.Domain.Documentos;
using Tsc.GestaoDocumentos.Domain.Logs;
using Tsc.GestaoDocumentos.Domain.Organizacoes;

namespace Tsc.GestaoDocumentos.Application.Documentos;

/// <summary>
/// Serviço de aplicação para gerenciamento de Documentos.
/// Responsável por orquestrar operações relacionadas a Documentos.
/// </summary>
public class ServicoAppDocumento(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ICurrentUserService currentUserService,
    IServicoAuditoria auditoriaService) : IServicoAppDocumento
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly ICurrentUserService _currentUserService = currentUserService;
    private readonly IServicoAuditoria _auditoriaService = auditoriaService;

    public async Task<DocumentoDto?> ObterPorIdAsync(IdDocumento id, CancellationToken cancellationToken = default)
    {
        var documento = await _unitOfWork.Documentos.ObterPorIdAsync(id, cancellationToken);
        return documento != null ? _mapper.Map<DocumentoDto>(documento) : null;
    }

    public async Task<PagedResult<DocumentoDto>> ObterTodosAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        var documentos = await _unitOfWork.Documentos.ObterTodosAsync(cancellationToken);
        var documentosDto = _mapper.Map<IEnumerable<DocumentoDto>>(documentos);

        var totalItems = documentosDto.Count();
        var items = documentosDto
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize);

        return new PagedResult<DocumentoDto>
        {
            Items = items,
            TotalItems = totalItems,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }

    public Task<DocumentoDto> CriarAsync(CreateDocumentoDto createDocumento, CancellationToken cancellationToken = default)
    {
        // TODO: Implementar lógica de criação após implementação do serviço de armazenamento
        throw new NotImplementedException("Implementação pendente após criação do IArmazenamentoService");
    }

    public Task<DocumentoDto> AtualizarAsync(IdDocumento id, UpdateDocumentoDto updateDocumento, CancellationToken cancellationToken = default)
    {
        // TODO: Implementar lógica de atualização
        throw new NotImplementedException("Implementação pendente após criação das entidades de domínio");
    }

    public Task<DocumentoDto> CriarNovaVersaoAsync(IdDocumento id, NovaVersaoDocumentoDto novaVersao, CancellationToken cancellationToken = default)
    {
        // TODO: Implementar lógica de nova versão
        throw new NotImplementedException("Implementação pendente após criação do IArmazenamentoService");
    }

    public Task<bool> RemoverAsync(IdDocumento id, CancellationToken cancellationToken = default)
    {
        // TODO: Implementar lógica de remoção
        throw new NotImplementedException("Implementação pendente após criação das entidades de domínio");
    }

    public Task<(Stream arquivo, string nomeArquivo, string tipoArquivo)?> FazerDownloadAsync(IdDocumento id, CancellationToken cancellationToken = default)
    {
        // TODO: Implementar lógica de download
        throw new NotImplementedException("Implementação pendente após criação do IArmazenamentoService");
    }
}
