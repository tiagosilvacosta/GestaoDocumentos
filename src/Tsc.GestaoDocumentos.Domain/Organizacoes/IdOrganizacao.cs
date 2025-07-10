using DddBase.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tsc.GestaoDocumentos.Domain.Organizacoes
{
    public record IdOrganizacao : IdEntidadeBase<Guid>
    {
        public IdOrganizacao(Guid valor) : base(valor)
        {
        }
        public static IdOrganizacao CriarNovo()
        {
            return new IdOrganizacao(Guid.NewGuid());
        }
    }
}
