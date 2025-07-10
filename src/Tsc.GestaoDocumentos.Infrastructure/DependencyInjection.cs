using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tsc.GestaoDocumentos.Domain.Common;
using Tsc.GestaoDocumentos.Domain.Documentos;
using Tsc.GestaoDocumentos.Domain.Logs;
using Tsc.GestaoDocumentos.Domain.Organizacoes;
using Tsc.GestaoDocumentos.Domain.Usuarios;
using Tsc.GestaoDocumentos.Infrastructure.Data;
using Tsc.GestaoDocumentos.Infrastructure.Documentos;
using Tsc.GestaoDocumentos.Infrastructure.Logs;
using Tsc.GestaoDocumentos.Infrastructure.Organizacoes;
using Tsc.GestaoDocumentos.Infrastructure.Repositories;
using Tsc.GestaoDocumentos.Infrastructure.Usuarios;

namespace Tsc.GestaoDocumentos.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Entity Framework
        services.AddDbContext<GestaoDocumentosDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        // Repositories
        services.AddScoped<IRepositorioOrganizacao, RepositorioOrganizacao>();
        services.AddScoped<IRepositorioUsuario, RepositorioUsuario>();
        services.AddScoped<IRepositorioTipoDono, RepositorioTipoDono>();
        services.AddScoped<IRepositorioTipoDocumento, RepositorioTipoDocumento>();
        services.AddScoped<IRepositorioDonoDocumento, RepositorioDonoDocumento>();
        services.AddScoped<IRepositorioDocumento, RepositorioDocumento>();
        services.AddScoped<IRepositorioLogAuditoria, RepositorioLogAuditoria>();

        // Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Services
        services.AddScoped<IServicoAuditoria, ServicoAuditoria>();
        services.AddScoped<IServicoCriptografia, ServicoCriptografia>();
        services.AddScoped<IServicoArmazenamentoArquivo, ServicoArmazenamentoArquivo>();

        return services;
    }
}
