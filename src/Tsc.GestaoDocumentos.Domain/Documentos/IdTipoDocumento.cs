using DddBase.Base;

namespace Tsc.GestaoDocumentos.Domain.Documentos
{
    public record IdTipoDocumento : IdEntidadeBase<Guid>
    {
        public IdTipoDocumento(Guid valor) : base(valor)
        {
        }
        public static IdTipoDocumento CriarNovo() => new(Guid.NewGuid());
    }
}