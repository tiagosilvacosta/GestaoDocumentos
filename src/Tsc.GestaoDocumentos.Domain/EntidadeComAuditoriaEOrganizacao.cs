using DddBase.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsc.GestaoDocumentos.Domain.Organizacoes;

namespace Tsc.GestaoDocumentos.Domain
{
    public  class EntidadeComAuditoriaEOrganizacao<TId> : EntidadeComAuditoria<TId>
        where TId : ObjetoDeValor
    {
        protected EntidadeComAuditoriaEOrganizacao() : base()
        {
        }

        protected EntidadeComAuditoriaEOrganizacao(TId id) : base(id)
        {
        }

        protected EntidadeComAuditoriaEOrganizacao(TId id, IdOrganizacao idOrganizacao) : base(id)
        {
            IdOrganizacao = idOrganizacao ?? throw new ArgumentNullException(nameof(idOrganizacao), "IdOrganizacao não pode ser nulo");
        }

        public IdOrganizacao IdOrganizacao { get; private set; } = null!;


    }
}
