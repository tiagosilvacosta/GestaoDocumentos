using Microsoft.EntityFrameworkCore.Storage;
using Tsc.GestaoDocumentos.Domain.Common;
using Tsc.GestaoDocumentos.Domain.Documentos;
using Tsc.GestaoDocumentos.Domain.Logs;
using Tsc.GestaoDocumentos.Domain.Organizacoes;
using Tsc.GestaoDocumentos.Domain.Usuarios;
using Tsc.GestaoDocumentos.Infrastructure.Data;

namespace Tsc.GestaoDocumentos.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly GestaoDocumentosDbContext _context;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(GestaoDocumentosDbContext context)
    {
        _context = context;
        
        Tenants = new TenantRepository(_context);
        Usuarios = new UsuarioRepository(_context);
        TiposDono = new TipoDonoRepository(_context);
        TiposDocumento = new TipoDocumentoRepository(_context);
        DonosDocumento = new DonoDocumentoRepository(_context);
        Documentos = new DocumentoRepository(_context);
        LogsAuditoria = new LogAuditoriaRepository(_context);
    }

    public ITenantRepository Tenants { get; }
    public IUsuarioRepository Usuarios { get; }
    public ITipoDonoRepository TiposDono { get; }
    public ITipoDocumentoRepository TiposDocumento { get; }
    public IDonoDocumentoRepository DonosDocumento { get; }
    public IDocumentoRepository Documentos { get; }
    public ILogAuditoriaRepository LogsAuditoria { get; }

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
