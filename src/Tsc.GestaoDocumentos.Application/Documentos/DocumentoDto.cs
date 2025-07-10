using Tsc.GestaoDocumentos.Application.DTOs.Common;
using Tsc.GestaoDocumentos.Domain.Documentos;

namespace Tsc.GestaoDocumentos.Application.Documentos;

public class DocumentoDto : OrganizacaoBaseDto
{
    public string NomeArquivo { get; set; } = string.Empty;
    public string ChaveArmazenamento { get; set; } = string.Empty;
    public DateTime DataUpload { get; set; }
    public long TamanhoArquivo { get; set; }
    public string TamanhoFormatado { get; set; } = string.Empty;
    public string TipoArquivo { get; set; } = string.Empty;
    public int Versao { get; set; }
    public string Status { get; set; } = string.Empty;
    public IdTipoDocumento IdTipoDocumento { get; set; } = null!;
    public string TipoDocumentoNome { get; set; } = string.Empty;
    public List<DonoDocumentoDto> DonosVinculados { get; set; } = [];
}

public class CreateDocumentoDto
{
    public string NomeArquivo { get; set; } = string.Empty;
    public Stream Arquivo { get; set; } = Stream.Null;
    public long TamanhoArquivo { get; set; }
    public string TipoArquivo { get; set; } = string.Empty;
    public IdTipoDocumento IdTipoDocumento { get; set; } = null!;
    public List<Guid> DonosDocumentoIds { get; set; } = [];
}

public class UpdateDocumentoDto
{
    public string NomeArquivo { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public List<Guid> DonosDocumentoIds { get; set; } = [];
}

public class NovaVersaoDocumentoDto
{
    public string NomeArquivo { get; set; } = string.Empty;
    public Stream Arquivo { get; set; } = Stream.Null;
    public long TamanhoArquivo { get; set; }
    public string TipoArquivo { get; set; } = string.Empty;
}
