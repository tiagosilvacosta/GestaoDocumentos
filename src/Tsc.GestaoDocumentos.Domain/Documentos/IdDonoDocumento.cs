using DddBase.Base;

namespace Tsc.GestaoDocumentos.Domain.Documentos
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