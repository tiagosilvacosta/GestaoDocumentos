using MediatR;
using Tsc.GestaoDocumentos.Application.DTOs;
using Tsc.GestaoDocumentos.Application.DTOs.Common;

namespace Tsc.GestaoDocumentos.Application.Queries.Tenants;

public record GetTenantByIdQuery(Guid Id) : IRequest<TenantDto?>;

public record GetTenantBySlugQuery(string Slug) : IRequest<TenantDto?>;

public record GetAllTenantsQuery(PagedRequest Request) : IRequest<PagedResult<TenantDto>>;
