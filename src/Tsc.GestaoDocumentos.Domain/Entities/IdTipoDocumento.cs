using DddBase.Base;

namespace Tsc.GestaoDocumentos.Domain.Entities
{
    public record IdTipoDocumento : IdEntidadeBase<Guid>
    {
        public IdTipoDocumento(Guid valor) : base(valor)
        {
        }
        public static IdTipoDocumento CriarNovo() => new(Guid.NewGuid());
    }
}