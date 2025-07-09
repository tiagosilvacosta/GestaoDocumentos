using Tsc.GestaoDocumentos.Domain.Entities;

namespace Tsc.GestaoDocumentos.Domain.Repositories;

public interface ITenantRepository : IBaseRepository<Tenant>
{
    Task<Tenant?> ObterPorSlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<bool> SlugExisteAsync(string slug, CancellationToken cancellationToken = default);
    Task<bool> SlugExisteAsync(string slug, Guid excluirId, CancellationToken cancellationToken = default);
}
