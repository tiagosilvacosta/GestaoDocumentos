using DddBase.Base;

namespace Tsc.GestaoDocumentos.Domain.Documentos
{
    public record IdDonoDocumento : IdEntidadeBase<Guid>
    {
        public IdDonoDocumento(Guid valor) : base(valor)
        {
            if (valor == Guid.Empty)
                throw new ArgumentException("O valor do ID não pode ser um GUID vazio.", nameof(valor));
        }
        public static IdDonoDocumento CriarNovo()
        {
            return new IdDonoDocumento(Guid.NewGuid());
        }
    }
}