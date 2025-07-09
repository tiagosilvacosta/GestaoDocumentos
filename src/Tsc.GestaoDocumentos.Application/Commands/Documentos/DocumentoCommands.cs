using MediatR;
using Tsc.GestaoDocumentos.Application.DTOs;

namespace Tsc.GestaoDocumentos.Application.Commands.Documentos;

public record CreateDocumentoCommand(CreateDocumentoDto Documento) : IRequest<DocumentoDto>;

public record UpdateDocumentoCommand(Guid Id, UpdateDocumentoDto Documento) : IRequest<DocumentoDto>;

public record DeleteDocumentoCommand(Guid Id) : IRequest<bool>;

public record NovaVersaoDocumentoCommand(Guid Id, NovaVersaoDocumentoDto NovaVersao) : IRequest<DocumentoDto>;

public record DownloadDocumentoCommand(Guid Id) : IRequest<(Stream Arquivo, string NomeArquivo, string TipoArquivo)>;
