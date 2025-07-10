using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Tsc.GestaoDocumentos.Application.Documentos;
using Tsc.GestaoDocumentos.Application.Mappings;
using Tsc.GestaoDocumentos.Application.Organizacoes;
using Tsc.GestaoDocumentos.Application.Usuarios;

namespace Tsc.GestaoDocumentos.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        // Serviços de Aplicação
        services.AddScoped<IServicoAppOrganizacao, ServicoAppOrganizacao>();
        services.AddScoped<IServicoAppUsuario, ServicoAppUsuario>();
        services.AddScoped<IServicoAppDocumento, ServicoAppDocumento>();
        services.AddScoped<IServicoAppDonoDocumento, ServicoAppDonoDocumento>();
        services.AddScoped<IServicoAppTipoDocumento, ServicoAppTipoDocumento>();
        services.AddScoped<IServicoAppTipoDono, ServicoAppTipoDono>();

        // AutoMapper
        services.AddAutoMapper(typeof(DomainToDtoProfile));

        // FluentValidation
        services.AddValidatorsFromAssembly(assembly);

        return services;
    }
}
