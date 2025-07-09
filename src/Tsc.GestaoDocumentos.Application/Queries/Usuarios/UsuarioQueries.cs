using MediatR;
using Tsc.GestaoDocumentos.Application.DTOs;
using Tsc.GestaoDocumentos.Application.DTOs.Common;

namespace Tsc.GestaoDocumentos.Application.Queries.Usuarios;

public record GetUsuarioByIdQuery(Guid Id) : IRequest<UsuarioDto?>;

public record GetUsuarioByEmailQuery(string Email) : IRequest<UsuarioDto?>;

public record GetUsuarioByLoginQuery(string Login) : IRequest<UsuarioDto?>;

public record GetAllUsuariosQuery(PagedRequest Request) : IRequest<PagedResult<UsuarioDto>>;

public record GetUsuariosByPerfilQuery(string Perfil, PagedRequest Request) : IRequest<PagedResult<UsuarioDto>>;
