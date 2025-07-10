using DddBase.Base;

namespace Tsc.GestaoDocumentos.Domain.Documentos
{
    public record IdDocumentoDonoDocumento : IdEntidadeBase<Guid>
    {
        public IdDocumentoDonoDocumento(Guid valor) : base(valor)
        {
        }

        public static IdDocumentoDonoDocumento CriarNovo()
        {
            return new IdDocumentoDonoDocumento(Guid.NewGuid());
        }
    }
}