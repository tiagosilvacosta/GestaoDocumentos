using DddBase.Base;

namespace Tsc.GestaoDocumentos.Domain.Documentos
{
    public record IdTipoDonoDocumento : IdEntidadeBase<Guid>
    {
        public IdTipoDonoDocumento(Guid valor) : base(valor)
        {

        }

        public static IdTipoDonoDocumento CriarNovo() => new(Guid.NewGuid());

    }
}