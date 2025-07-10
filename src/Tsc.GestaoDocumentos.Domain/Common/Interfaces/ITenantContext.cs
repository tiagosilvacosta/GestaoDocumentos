using Tsc.GestaoDocumentos.Domain.Organizacoes;

namespace Tsc.GestaoDocumentos.Domain.Common.Interfaces;

public interface ITenantContext
{
    IdOrganizacao IdOrganizacao { get; }
    string TenantSlug { get; }
}

public interface ICurrentUserService
{
    Guid UserId { get; }
    string UserName { get; }
    string Email { get; }
    IdOrganizacao idOrganizacao { get; }
}
