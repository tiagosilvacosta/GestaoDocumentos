using DddBase.Base;
using DddBase.Repositorio;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Tsc.GestaoDocumentos.Domain;
using Tsc.GestaoDocumentos.Domain.Common;
using Tsc.GestaoDocumentos.Domain.Organizacoes;
using Tsc.GestaoDocumentos.Infrastructure.Data;

namespace Tsc.GestaoDocumentos.Infrastructure.Repositories;

/// <summary>
/// Implementação base do repositório usando Entity Framework.
/// </summary>
/// <typeparam name="T">Tipo da entidade que deve herdar de AggregateRoot</typeparam>
public class RepositorioBase<T, TId>(GestaoDocumentosDbContext context) : IRepositorio<T, TId> 
    where T : EntidadeBase<TId>, IRaizAgregado
    where TId : ObjetoDeValor
{
    protected readonly GestaoDocumentosDbContext _context = context;
    protected readonly DbSet<T> _dbSet = context.Set<T>();

    public virtual async Task<T?> ObterPorIdAsync(TId id, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FindAsync([id], cancellationToken);
    }

    public virtual async Task<IEnumerable<T>> ObterPorCondicaoAsync(Expression<Func<T, bool>> predicado, CancellationToken cancellationToken = default)
    {
        return await _dbSet.Where(predicado).ToListAsync(cancellationToken);
    }

    public virtual async Task<IEnumerable<T>> ObterTodosAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet.ToListAsync(cancellationToken);
    }

    public virtual async Task<T> AdicionarAsync(T entidade, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entidade, cancellationToken);
        return entidade;
    }

    public virtual async Task<T> AtualizarAsync(T entidade, CancellationToken cancellationToken = default)
    {
        _dbSet.Update(entidade);
        await Task.CompletedTask;
        return entidade;
    }

    public virtual async Task<bool> RemoverAsync(T entidade, CancellationToken cancellationToken = default)
    {
        _dbSet.Remove(entidade);
        await Task.CompletedTask;
        return true;
    }

    public virtual async Task<bool> RemoverAsync(TId id, CancellationToken cancellationToken = default)
    {
        var entity = await ObterPorIdAsync(id, cancellationToken);
        if (entity == null) return false;
        
        _dbSet.Remove(entity);
        await Task.CompletedTask;
        return true;
    }

    public virtual async Task<bool> ExisteAsync(TId id, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FindAsync([id], cancellationToken) != null;
    }

    public virtual async Task<int> ContarAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet.CountAsync(cancellationToken);
    }

    public virtual async Task<int> ContarAsync(Expression<Func<T, bool>> predicado, CancellationToken cancellationToken = default)
    {
        return await _dbSet.CountAsync(predicado, cancellationToken);
    }
}

/// <summary>
/// Implementação base do repositório para entidades multi-tenant.
/// </summary>
/// <typeparam name="T">Tipo da entidade que deve herdar de TenantEntity</typeparam>
public class RepositorioBaseComOrganizacao<T, TId>(GestaoDocumentosDbContext context) : RepositorioBase<T, TId>(context), IRepositorioComOrganizacao<T, TId> 
    where T : EntidadeComAuditoriaEOrganizacao<TId>, IRaizAgregado
    where TId : ObjetoDeValor
{
    public virtual async Task<T?> ObterPorIdETOrganizacaoAsync(TId id, IdOrganizacao idOrganizacao, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(e => e.Id == id && e.IdOrganizacao == idOrganizacao)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<IEnumerable<T>> ObterPorTenanteAsync(IdOrganizacao idOrganizacao, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(e => e.IdOrganizacao == idOrganizacao)
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<bool> ExisteComTenanteAsync(TId id, IdOrganizacao idOrganizacao, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AnyAsync(e => e.Id == id && e.IdOrganizacao == idOrganizacao, cancellationToken);
    }
}
