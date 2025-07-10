using Tsc.GestaoDocumentos.Domain.Usuarios;

namespace Tsc.GestaoDocumentos.Domain.Organizacoes;

public interface IContextoOrganizacao
{
    IdOrganizacao IdOrganizacao { get; }
    string TenantSlug { get; }
}

public interface ICurrentUserService
{
    IdUsuario IdUsuario { get; }
    string UserName { get; }
    string Email { get; }
    IdOrganizacao IdOrganizacao { get; }
}
