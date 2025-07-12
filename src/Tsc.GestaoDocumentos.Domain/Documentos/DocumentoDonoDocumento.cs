using DddBase.Base;
using Tsc.GestaoDocumentos.Domain.Organizacoes;

namespace Tsc.GestaoDocumentos.Domain.Documentos;

public class DocumentoDonoDocumento : EntidadeComAuditoriaEOrganizacao<IdDocumentoDonoDocumento>, IRaizAgregado
{
    public IdDocumento IdDocumento { get; private set; } = null!;
    public IdDonoDocumento IdDonoDocumento { get; private set; } = null!;

    // Navegação
    public Documento Documento { get; private set; } = null!;
    public DonoDocumento DonoDocumento { get; private set; } = null!;

    protected DocumentoDonoDocumento() : base() { }

    public DocumentoDonoDocumento(Documento documento, DonoDocumento donoDocumento, IdOrganizacao idOrganizacao)
        : base(IdDocumentoDonoDocumento.CriarNovo(), idOrganizacao)
    {
        IdDocumento = documento.Id;
        Documento = documento;
        IdDonoDocumento = donoDocumento.Id;
        DonoDocumento = donoDocumento;
    }
}
