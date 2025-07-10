using Microsoft.EntityFrameworkCore.Storage;
using Tsc.GestaoDocumentos.Domain.Common;
using Tsc.GestaoDocumentos.Domain.Documentos;
using Tsc.GestaoDocumentos.Domain.Logs;
using Tsc.GestaoDocumentos.Domain.Organizacoes;
using Tsc.GestaoDocumentos.Domain.Usuarios;
using Tsc.GestaoDocumentos.Infrastructure.Data;
using Tsc.GestaoDocumentos.Infrastructure.Documentos;
using Tsc.GestaoDocumentos.Infrastructure.Logs;
using Tsc.GestaoDocumentos.Infrastructure.Organizacoes;
using Tsc.GestaoDocumentos.Infrastructure.Usuarios;

namespace Tsc.GestaoDocumentos.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly GestaoDocumentosDbContext _context;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(GestaoDocumentosDbContext context)
    {
        _context = context;
        
        Tenants = new RepositorioOrganizacao(_context);
        Usuarios = new RepositorioUsuario(_context);
        TiposDono = new RepositorioTipoDono(_context);
        TiposDocumento = new RepositorioTipoDocumento(_context);
        DonosDocumento = new RepositorioDonoDocumento(_context);
        Documentos = new RepositorioDocumento(_context);
        LogsAuditoria = new RepositorioLogAuditoria(_context);
    }

    public IRepositorioOrganizacao Tenants { get; }
    public IRepositorioUsuario Usuarios { get; }
    public IRepositorioTipoDono TiposDono { get; }
    public IRepositorioTipoDocumento TiposDocumento { get; }
    public IRepositorioDonoDocumento DonosDocumento { get; }
    public IRepositorioDocumento Documentos { get; }
    public IRepositorioLogAuditoria LogsAuditoria { get; }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}
