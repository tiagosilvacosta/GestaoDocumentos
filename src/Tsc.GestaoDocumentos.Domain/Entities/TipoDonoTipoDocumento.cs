using Tsc.GestaoDocumentos.Domain.Common;

namespace Tsc.GestaoDocumentos.Domain.Entities;

public class TipoDonoTipoDocumento : TenantEntity
{
    public Guid TipoDonoId { get; private set; }
    public Guid TipoDocumentoId { get; private set; }

    // Navegação
    public TipoDono TipoDono { get; private set; } = null!;
    public TipoDocumento TipoDocumento { get; private set; } = null!;

    protected TipoDonoTipoDocumento() : base() { }

    public TipoDonoTipoDocumento(Guid tipoDonoId, Guid tipoDocumentoId, Guid tenantId)
        : base(tenantId)
    {
        TipoDonoId = tipoDonoId;
        TipoDocumentoId = tipoDocumentoId;
    }
}
