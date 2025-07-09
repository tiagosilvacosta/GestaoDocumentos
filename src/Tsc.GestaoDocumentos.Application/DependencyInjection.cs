using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Tsc.GestaoDocumentos.Application.Mappings;

namespace Tsc.GestaoDocumentos.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        // MediatR
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));

        // AutoMapper
        services.AddAutoMapper(typeof(DomainToDtoProfile));

        // FluentValidation
        services.AddValidatorsFromAssembly(assembly);

        return services;
    }
}
