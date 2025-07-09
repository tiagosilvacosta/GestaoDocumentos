namespace Tsc.GestaoDocumentos.Domain.Common.Interfaces;

public interface ITenantContext
{
    Guid TenantId { get; }
    string TenantSlug { get; }
}

public interface ICurrentUserService
{
    Guid UserId { get; }
    string UserName { get; }
    string Email { get; }
    Guid TenantId { get; }
}
