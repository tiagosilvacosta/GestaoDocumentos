using MediatR;
using Tsc.GestaoDocumentos.Application.DTOs;

namespace Tsc.GestaoDocumentos.Application.Commands.Usuarios;

public record CreateUsuarioCommand(CreateUsuarioDto Usuario) : IRequest<UsuarioDto>;

public record UpdateUsuarioCommand(Guid Id, UpdateUsuarioDto Usuario) : IRequest<UsuarioDto>;

public record DeleteUsuarioCommand(Guid Id) : IRequest<bool>;

public record AlterarSenhaUsuarioCommand(Guid Id, AlterarSenhaDto AlterarSenha) : IRequest<bool>;
