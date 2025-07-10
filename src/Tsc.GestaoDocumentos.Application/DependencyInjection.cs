using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Tsc.GestaoDocumentos.Application.Mappings;
using Tsc.GestaoDocumentos.Application.Services;

namespace Tsc.GestaoDocumentos.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        // Serviços de Aplicação
        services.AddScoped<ITenantAppService, TenantAppService>();
        services.AddScoped<IUsuarioAppService, UsuarioAppService>();
        services.AddScoped<IDocumentoAppService, DocumentoAppService>();
        services.AddScoped<IDonoDocumentoAppService, DonoDocumentoAppService>();
        services.AddScoped<ITipoDocumentoAppService, TipoDocumentoAppService>();
        services.AddScoped<ITipoDonoAppService, TipoDonoAppService>();

        // AutoMapper
        services.AddAutoMapper(typeof(DomainToDtoProfile));

        // FluentValidation
        services.AddValidatorsFromAssembly(assembly);

        return services;
    }
}
