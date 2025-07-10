using DddBase.Base;

namespace Tsc.GestaoDocumentos.Domain.Logs
{
    public record IdLogAuditoria : IdEntidadeBase<Guid>
    {
        public IdLogAuditoria(Guid valor) : base(valor)
        {

        }

        public static IdLogAuditoria CriarNovo() => new(Guid.NewGuid());
    }
}