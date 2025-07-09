using Tsc.GestaoDocumentos.Domain.Common;

namespace Tsc.GestaoDocumentos.Domain.Entities;

public class DocumentoDonoDocumento : TenantEntity
{
    public Guid DocumentoId { get; private set; }
    public Guid DonoDocumentoId { get; private set; }

    // Navegação
    public Documento Documento { get; private set; } = null!;
    public DonoDocumento DonoDocumento { get; private set; } = null!;

    protected DocumentoDonoDocumento() : base() { }

    public DocumentoDonoDocumento(Guid documentoId, Guid donoDocumentoId, Guid tenantId)
        : base(tenantId)
    {
        DocumentoId = documentoId;
        DonoDocumentoId = donoDocumentoId;
    }
}
