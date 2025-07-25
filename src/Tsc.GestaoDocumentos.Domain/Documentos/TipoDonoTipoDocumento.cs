using DddBase.Base;
using Tsc.GestaoDocumentos.Domain.Common;
using Tsc.GestaoDocumentos.Domain.Organizacoes;

namespace Tsc.GestaoDocumentos.Domain.Documentos;

public class TipoDonoTipoDocumento : EntidadeComAuditoriaEOrganizacao<IdTipoDonoTipoDocumento>, IRaizAgregado
{
    public IdTipoDono IdTipoDono { get; private set; } = null!;
    public IdTipoDocumento IdTipoDocumento { get; private set; } = null!;

    // Navegação
    public TipoDono TipoDono { get; private set; } = null!;
    public TipoDocumento TipoDocumento { get; private set; } = null!;

    protected TipoDonoTipoDocumento() : base() { }

    public TipoDonoTipoDocumento(IdTipoDono tipoDonoId, IdTipoDocumento tipoDocumentoId, IdOrganizacao idOrganizacao)
        : base(IdTipoDonoTipoDocumento.CriarNovo(), idOrganizacao)
    {
        IdTipoDono = tipoDonoId;
        IdTipoDocumento = tipoDocumentoId;
    }
}
