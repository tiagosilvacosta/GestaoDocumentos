using DddBase.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tsc.GestaoDocumentos.Domain.Documentos
{
    public record IdTipoDonoTipoDocumento : IdEntidadeBase<Guid>
    {
        public IdTipoDonoTipoDocumento(Guid valor) : base(valor)
        {
        }

        public static IdTipoDonoTipoDocumento CriarNovo()
        {
            return new IdTipoDonoTipoDocumento(Guid.NewGuid());
        }
    }
}
