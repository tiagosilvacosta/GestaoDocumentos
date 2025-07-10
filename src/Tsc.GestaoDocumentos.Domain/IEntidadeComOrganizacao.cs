using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsc.GestaoDocumentos.Domain.Organizacoes;

namespace Tsc.GestaoDocumentos.Domain
{
    public interface IEntidadeComOrganizacao
    {
        public void AlterarOrganizacao(IdOrganizacao idOrganizacao);
        public IdOrganizacao IdOrganizacao { get; }
    }
}
