using Tsc.GestaoDocumentos.Application.DTOs.Common;

namespace Tsc.GestaoDocumentos.Application.Documentos;

public class TipoDonoDto : OrganizacaoBaseDto
{
    public string Nome { get; set; } = string.Empty;
    public List<TipoDocumentoDto> TiposDocumentoVinculados { get; set; } = new();
}

public class CreateTipoDonoDto
{
    public string Nome { get; set; } = string.Empty;
}

public class UpdateTipoDonoDto
{
    public string Nome { get; set; } = string.Empty;
}

public class VincularTipoDocumentoDto
{
    public List<Guid> TiposDocumentoIds { get; set; } = new();
}
