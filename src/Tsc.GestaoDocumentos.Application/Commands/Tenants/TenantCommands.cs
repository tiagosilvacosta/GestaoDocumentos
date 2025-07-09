using MediatR;
using Tsc.GestaoDocumentos.Application.DTOs;

namespace Tsc.GestaoDocumentos.Application.Commands.Tenants;

public record CreateTenantCommand(CreateTenantDto Tenant) : IRequest<TenantDto>;

public record UpdateTenantCommand(Guid Id, UpdateTenantDto Tenant) : IRequest<TenantDto>;

public record DeleteTenantCommand(Guid Id) : IRequest<bool>;
