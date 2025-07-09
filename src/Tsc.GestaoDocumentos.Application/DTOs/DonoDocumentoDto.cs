using Tsc.GestaoDocumentos.Application.DTOs.Common;

namespace Tsc.GestaoDocumentos.Application.DTOs;

public class DonoDocumentoDto : TenantBaseDto
{
    public string NomeAmigavel { get; set; } = string.Empty;
    public Guid TipoDonoId { get; set; }
    public string TipoDonoNome { get; set; } = string.Empty;
    public List<DocumentoDto> DocumentosVinculados { get; set; } = new();
}

public class CreateDonoDocumentoDto
{
    public string NomeAmigavel { get; set; } = string.Empty;
    public Guid TipoDonoId { get; set; }
}

public class UpdateDonoDocumentoDto
{
    public string NomeAmigavel { get; set; } = string.Empty;
    public Guid TipoDonoId { get; set; }
}
