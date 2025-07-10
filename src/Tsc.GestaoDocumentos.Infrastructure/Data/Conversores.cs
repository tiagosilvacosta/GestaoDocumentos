using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Tsc.GestaoDocumentos.Domain.Documentos;
using Tsc.GestaoDocumentos.Domain.Logs;
using Tsc.GestaoDocumentos.Domain.Organizacoes;
using Tsc.GestaoDocumentos.Domain.Usuarios;

namespace Tsc.GestaoDocumentos.Infrastructure.Data
{
    public class IdDonoDocumentoConverter : ValueConverter<IdDonoDocumento, Guid>
    {
        public IdDonoDocumentoConverter()
            : base(
                v => v.Valor,
                v => new IdDonoDocumento(v))
        {
        }
    }

    public class IdDocumentoConverter : ValueConverter<IdDocumento, Guid>
    {
        public IdDocumentoConverter()
            : base(
                v => v.Valor,
                v => new IdDocumento(v))
        {
        }
    }

    public class IdOrganizacaoConverter : ValueConverter<IdOrganizacao, Guid>
    {
        public IdOrganizacaoConverter()
            : base(
                v => v.Valor,
                v => new IdOrganizacao(v))
        {
        }
    }

    public class IdTipoDonoConverter : ValueConverter<IdTipoDono, Guid>
    {
        public IdTipoDonoConverter()
            : base(
                v => v.Valor,
                v => new IdTipoDono(v))
        {
        }
    }
    public class IdTipoDocumentoConverter : ValueConverter<IdTipoDocumento, Guid>
    {
        public IdTipoDocumentoConverter()
            : base(
                v => v.Valor,
                v => new IdTipoDocumento(v))
        {
        }
    }
    
    public class IdDocumentoDonoDocumentoConverter : ValueConverter<IdDocumentoDonoDocumento, Guid>
    {
        public IdDocumentoDonoDocumentoConverter()
            : base(
                v => v.Valor,
                v => new IdDocumentoDonoDocumento(v))
        {
        }
    }

    public class IdUsuarioConverter : ValueConverter<IdUsuario, Guid>
    {
        public IdUsuarioConverter()
            : base(
                v => v.Valor,
                v => new IdUsuario(v))
        {
        }
    }

    public class IdTipoDonoTipoDocumentoConverter : ValueConverter<IdTipoDonoTipoDocumento, Guid>
    {
        public IdTipoDonoTipoDocumentoConverter()
            : base(
                v => v.Valor,
                v => new IdTipoDonoTipoDocumento(v))
        {
        }
    }

    public class IdLogAuditoriaConverter : ValueConverter<IdLogAuditoria, Guid>
    {
        public IdLogAuditoriaConverter()
            : base(
                v => v.Valor,
                v => new IdLogAuditoria(v))
        {
        }
    }
}
