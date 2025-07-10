using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Tsc.GestaoDocumentos.Domain.Common;
using Tsc.GestaoDocumentos.Domain.Repositories;
using Tsc.GestaoDocumentos.Infrastructure.Data;

namespace Tsc.GestaoDocumentos.Infrastructure.Repositories;

/// <summary>
/// Implementação base do repositório usando Entity Framework.
/// </summary>
/// <typeparam name="T">Tipo da entidade que deve herdar de AggregateRoot</typeparam>
public class BaseRepository<T> : IBaseRepository<T> where T : AggregateRoot
{
    protected readonly GestaoDocumentosDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public BaseRepository(GestaoDocumentosDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public virtual async Task<T?> ObterPorIdAsync(EntityId id, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FindAsync(new object[] { id.Valor }, cancellationToken);
    }

    public virtual async Task<IEnumerable<T>> ObterPorCondicaoAsync(Expression<Func<T, bool>> condicao, CancellationToken cancellationToken = default)
    {
        return await _dbSet.Where(condicao).ToListAsync(cancellationToken);
    }

    public virtual async Task<IEnumerable<T>> ObterTodosAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet.ToListAsync(cancellationToken);
    }

    public virtual async Task<T> AdicionarAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
        return entity;
    }

    public virtual async Task<T> AtualizarAsync(T entity, CancellationToken cancellationToken = default)
    {
        _dbSet.Update(entity);
        await Task.CompletedTask;
        return entity;
    }

    public virtual async Task<bool> RemoverAsync(T entity, CancellationToken cancellationToken = default)
    {
        _dbSet.Remove(entity);
        await Task.CompletedTask;
        return true;
    }

    public virtual async Task<bool> RemoverAsync(EntityId id, CancellationToken cancellationToken = default)
    {
        var entity = await ObterPorIdAsync(id, cancellationToken);
        if (entity == null) return false;
        
        _dbSet.Remove(entity);
        await Task.CompletedTask;
        return true;
    }

    public virtual async Task<bool> ExisteAsync(EntityId id, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FindAsync(new object[] { id.Valor }, cancellationToken) != null;
    }

    public virtual async Task<int> ContarAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet.CountAsync(cancellationToken);
    }

    public virtual async Task<int> ContarAsync(Expression<Func<T, bool>> condicao, CancellationToken cancellationToken = default)
    {
        return await _dbSet.CountAsync(condicao, cancellationToken);
    }
}

/// <summary>
/// Implementação base do repositório para entidades multi-tenant.
/// </summary>
/// <typeparam name="T">Tipo da entidade que deve herdar de TenantEntity</typeparam>
public class TenantBaseRepository<T> : BaseRepository<T>, ITenantRepository<T> 
    where T : TenantEntity
{
    public TenantBaseRepository(GestaoDocumentosDbContext context) : base(context)
    {
    }

    public virtual async Task<T?> ObterPorIdETenanteAsync(EntityId id, EntityId tenantId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(e => e.Id.Valor == id.Valor && e.TenantId == tenantId.Valor)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<IEnumerable<T>> ObterPorTenanteAsync(EntityId tenantId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(e => e.TenantId == tenantId.Valor)
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<bool> ExisteComTenanteAsync(EntityId id, EntityId tenantId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AnyAsync(e => e.Id.Valor == id.Valor && e.TenantId == tenantId.Valor, cancellationToken);
    }
}
