using MediatR;
using Tsc.GestaoDocumentos.Application.DTOs;
using Tsc.GestaoDocumentos.Application.DTOs.Common;

namespace Tsc.GestaoDocumentos.Application.Queries.Documentos;

public record GetDocumentoByIdQuery(Guid Id) : IRequest<DocumentoDto?>;

public record GetAllDocumentosQuery(PagedRequest Request) : IRequest<PagedResult<DocumentoDto>>;

public record GetDocumentosByTipoQuery(Guid TipoDocumentoId, PagedRequest Request) : IRequest<PagedResult<DocumentoDto>>;

public record GetDocumentosByDonoQuery(Guid DonoDocumentoId, PagedRequest Request) : IRequest<PagedResult<DocumentoDto>>;

public record GetDocumentosAtivosQuery(PagedRequest Request) : IRequest<PagedResult<DocumentoDto>>;

public record GetVersoesDocumentoQuery(Guid DocumentoId) : IRequest<IEnumerable<DocumentoDto>>;
