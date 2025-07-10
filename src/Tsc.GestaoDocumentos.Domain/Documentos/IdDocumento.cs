using DddBase.Base;

namespace Tsc.GestaoDocumentos.Domain.Documentos
{
    public record IdDocumento : IdEntidadeBase<Guid>
    {
        public IdDocumento(Guid valor) : base(valor)
        {
        }

        public static IdDocumento CriarNovo()
        {
            return new IdDocumento(Guid.NewGuid());
        }
    }
}