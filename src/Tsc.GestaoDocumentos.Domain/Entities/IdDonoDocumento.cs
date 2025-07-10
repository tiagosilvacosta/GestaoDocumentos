using DddBase.Base;

namespace Tsc.GestaoDocumentos.Domain.Entities
{
    public record IdDonoDocumento : IdEntidadeBase<Guid>
    {
        public IdDonoDocumento(Guid valor) : base(valor)
        {
        }
        public static IdDonoDocumento CriarNovo()
        {
            return new IdDonoDocumento(Guid.NewGuid());
        }
    }   
}