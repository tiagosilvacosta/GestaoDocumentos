using Tsc.GestaoDocumentos.Application.DTOs.Common;

namespace Tsc.GestaoDocumentos.Application.DTOs;

public class TipoDocumentoDto : TenantBaseDto
{
    public string Nome { get; set; } = string.Empty;
    public bool PermiteMultiplosDocumentos { get; set; }
    public List<TipoDonoDto> TiposDonoVinculados { get; set; } = new();
}

public class CreateTipoDocumentoDto
{
    public string Nome { get; set; } = string.Empty;
    public bool PermiteMultiplosDocumentos { get; set; }
}

public class UpdateTipoDocumentoDto
{
    public string Nome { get; set; } = string.Empty;
    public bool PermiteMultiplosDocumentos { get; set; }
}

public class VincularTipoDonoDto
{
    public List<Guid> TiposDonoIds { get; set; } = new();
}
