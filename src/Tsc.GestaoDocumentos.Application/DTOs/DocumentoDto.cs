using Tsc.GestaoDocumentos.Application.DTOs.Common;

namespace Tsc.GestaoDocumentos.Application.DTOs;

public class DocumentoDto : TenantBaseDto
{
    public string NomeArquivo { get; set; } = string.Empty;
    public string ChaveArmazenamento { get; set; } = string.Empty;
    public DateTime DataUpload { get; set; }
    public long TamanhoArquivo { get; set; }
    public string TamanhoFormatado { get; set; } = string.Empty;
    public string TipoArquivo { get; set; } = string.Empty;
    public int Versao { get; set; }
    public string Status { get; set; } = string.Empty;
    public IdTipoDocumento idTipoDocumento { get; set; }
    public string TipoDocumentoNome { get; set; } = string.Empty;
    public List<DonoDocumentoDto> DonosVinculados { get; set; } = new();
}

public class CreateDocumentoDto
{
    public string NomeArquivo { get; set; } = string.Empty;
    public Stream Arquivo { get; set; } = Stream.Null;
    public long TamanhoArquivo { get; set; }
    public string TipoArquivo { get; set; } = string.Empty;
    public IdTipoDocumento idTipoDocumento { get; set; }
    public List<Guid> DonosDocumentoIds { get; set; } = new();
}

public class UpdateDocumentoDto
{
    public string NomeArquivo { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public List<Guid> DonosDocumentoIds { get; set; } = new();
}

public class NovaVersaoDocumentoDto
{
    public string NomeArquivo { get; set; } = string.Empty;
    public Stream Arquivo { get; set; } = Stream.Null;
    public long TamanhoArquivo { get; set; }
    public string TipoArquivo { get; set; } = string.Empty;
}
