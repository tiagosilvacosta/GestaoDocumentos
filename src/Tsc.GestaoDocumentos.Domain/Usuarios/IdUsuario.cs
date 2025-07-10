using DddBase.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tsc.GestaoDocumentos.Domain.Usuarios
{
    public record IdUsuario : IdEntidadeBase<Guid>
    {
        public IdUsuario(Guid valor) : base(valor)
        {
        }

        public static IdUsuario GerarNovo()
        {
            return new IdUsuario(Guid.NewGuid());
        }
    }
}
