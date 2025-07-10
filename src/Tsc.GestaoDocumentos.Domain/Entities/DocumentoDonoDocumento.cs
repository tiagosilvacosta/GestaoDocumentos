using DddBase.Base;
using Tsc.GestaoDocumentos.Domain.Organizacoes;
using Tsc.GestaoDocumentos.Domain.Repositories;

namespace Tsc.GestaoDocumentos.Domain.Entities;

public class DocumentoDonoDocumento : EntidadeComAuditoriaEOrganizacao<IdDocumentoDonoDocumento>, IRaizAgregado
{
    public IdDocumento IdDocumento { get; private set; } = null!;
    public IdDonoDocumento IdDonoDocumento { get; private set; } = null!;

    // Navegação
    public Documento Documento { get; private set; } = null!;
    public DonoDocumento DonoDocumento { get; private set; } = null!;

    protected DocumentoDonoDocumento() : base() { }

    public DocumentoDonoDocumento(IdDocumento idDocumento, IdDonoDocumento idDonoDocumento, IdOrganizacao idOrganizacao)
        : base(IdDocumentoDonoDocumento.CriarNovo(), idOrganizacao)
    {
        IdDocumento = idDocumento;
        IdDonoDocumento = idDonoDocumento;
    }
}
