using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Tsc.GestaoDocumentos.Domain.Organizacoes;
using Tsc.GestaoDocumentos.Domain.Usuarios;
using Tsc.GestaoDocumentos.Infrastructure.Data;
using Tsc.GestaoDocumentos.Infrastructure.Tests.Usuarios.Helpers;
using Tsc.GestaoDocumentos.Infrastructure.Usuarios;
using Xunit;

namespace Tsc.GestaoDocumentos.Infrastructure.Tests.Usuarios;

public sealed class RepositorioUsuarioTests : IDisposable
{
    private readonly GestaoDocumentosDbContext _context;
    private readonly RepositorioUsuario _repositorio;

    public RepositorioUsuarioTests()
    {
        _context = UsuarioTestHelper.CriarDbContextEmMemoria();
        _repositorio = new RepositorioUsuario(_context);
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    #region Testes ObterPorEmailAsync

    [Fact]
    public async Task ObterPorEmailAsync_DeveRetornarUsuario_QuandoEmailExisteNaOrganizacao()
    {
        // Arrange
        var organizacao = IdOrganizacao.CriarNovo();
        var usuario = UsuarioTestHelper.CriarUsuarioComEmailELogin("teste@exemplo.com", "teste.usuario", organizacao);
        await _context.Usuarios.AddAsync(usuario);
        await _context.SaveChangesAsync();

        // Act
        var resultado = await _repositorio.ObterPorEmailAsync("teste@exemplo.com", organizacao);

        // Assert
        resultado.Should().NotBeNull();
        resultado!.Email.Should().Be("teste@exemplo.com");
        resultado.IdOrganizacao.Should().Be(organizacao);
    }

    [Fact]
    public async Task ObterPorEmailAsync_DeveRetornarNull_QuandoEmailNaoExiste()
    {
        // Arrange
        var organizacao = IdOrganizacao.CriarNovo();

        // Act
        var resultado = await _repositorio.ObterPorEmailAsync("inexistente@exemplo.com", organizacao);

        // Assert
        resultado.Should().BeNull();
    }

    [Fact]
    public async Task ObterPorEmailAsync_DeveRetornarNull_QuandoEmailExisteEmOutraOrganizacao()
    {
        // Arrange
        var organizacao1 = IdOrganizacao.CriarNovo();
        var organizacao2 = IdOrganizacao.CriarNovo();
        var usuario = UsuarioTestHelper.CriarUsuarioComEmailELogin("teste@exemplo.com", "teste.usuario", organizacao1);
        await _context.Usuarios.AddAsync(usuario);
        await _context.SaveChangesAsync();

        // Act
        var resultado = await _repositorio.ObterPorEmailAsync("teste@exemplo.com", organizacao2);

        // Assert
        resultado.Should().BeNull();
    }

    [Fact]
    public async Task ObterPorEmailAsync_DeveSerCaseInsensitive()
    {
        // Arrange
        var organizacao = IdOrganizacao.CriarNovo();
        var usuario = UsuarioTestHelper.CriarUsuarioComEmailELogin("teste@exemplo.com", "teste.usuario", organizacao);
        await _context.Usuarios.AddAsync(usuario);
        await _context.SaveChangesAsync();

        // Act
        var resultado = await _repositorio.ObterPorEmailAsync("TESTE@EXEMPLO.COM", organizacao);

        // Assert
        resultado.Should().NotBeNull();
        resultado!.Email.Should().Be("teste@exemplo.com");
    }

    [Fact]
    public async Task ObterPorEmailAsync_DeveTratarEmailComEspacos()
    {
        // Arrange
        var organizacao = IdOrganizacao.CriarNovo();
        var usuario = UsuarioTestHelper.CriarUsuarioComEmailELogin("teste@exemplo.com", "teste.usuario", organizacao);
        await _context.Usuarios.AddAsync(usuario);
        await _context.SaveChangesAsync();

        // Act
        var resultado = await _repositorio.ObterPorEmailAsync(" teste@exemplo.com ", organizacao);

        // Assert
        resultado.Should().BeNull(); // Não deve encontrar com espaços
    }

    #endregion

    #region Testes ObterPorLoginAsync

    [Fact]
    public async Task ObterPorLoginAsync_DeveRetornarUsuario_QuandoLoginExisteNaOrganizacao()
    {
        // Arrange
        var organizacao = IdOrganizacao.CriarNovo();
        var usuario = UsuarioTestHelper.CriarUsuarioComEmailELogin("teste@exemplo.com", "teste.usuario", organizacao);
        await _context.Usuarios.AddAsync(usuario);
        await _context.SaveChangesAsync();

        // Act
        var resultado = await _repositorio.ObterPorLoginAsync("teste.usuario", organizacao);

        // Assert
        resultado.Should().NotBeNull();
        resultado!.Login.Should().Be("teste.usuario");
        resultado.IdOrganizacao.Should().Be(organizacao);
    }

    [Fact]
    public async Task ObterPorLoginAsync_DeveRetornarNull_QuandoLoginNaoExiste()
    {
        // Arrange
        var organizacao = IdOrganizacao.CriarNovo();

        // Act
        var resultado = await _repositorio.ObterPorLoginAsync("inexistente.usuario", organizacao);

        // Assert
        resultado.Should().BeNull();
    }

    [Fact]
    public async Task ObterPorLoginAsync_DeveRetornarNull_QuandoLoginExisteEmOutraOrganizacao()
    {
        // Arrange
        var organizacao1 = IdOrganizacao.CriarNovo();
        var organizacao2 = IdOrganizacao.CriarNovo();
        var usuario = UsuarioTestHelper.CriarUsuarioComEmailELogin("teste@exemplo.com", "teste.usuario", organizacao1);
        await _context.Usuarios.AddAsync(usuario);
        await _context.SaveChangesAsync();

        // Act
        var resultado = await _repositorio.ObterPorLoginAsync("teste.usuario", organizacao2);

        // Assert
        resultado.Should().BeNull();
    }

    [Fact]
    public async Task ObterPorLoginAsync_DeveSerCaseInsensitive()
    {
        // Arrange
        var organizacao = IdOrganizacao.CriarNovo();
        var usuario = UsuarioTestHelper.CriarUsuarioComEmailELogin("teste@exemplo.com", "teste.usuario", organizacao);
        await _context.Usuarios.AddAsync(usuario);
        await _context.SaveChangesAsync();

        // Act
        var resultado = await _repositorio.ObterPorLoginAsync("TESTE.USUARIO", organizacao);

        // Assert
        resultado.Should().NotBeNull();
        resultado!.Login.Should().Be("teste.usuario");
    }

    [Fact]
    public async Task ObterPorLoginAsync_DeveTratarLoginComEspacos()
    {
        // Arrange
        var organizacao = IdOrganizacao.CriarNovo();
        var usuario = UsuarioTestHelper.CriarUsuarioComEmailELogin("teste@exemplo.com", "teste.usuario", organizacao);
        await _context.Usuarios.AddAsync(usuario);
        await _context.SaveChangesAsync();

        // Act
        var resultado = await _repositorio.ObterPorLoginAsync(" teste.usuario ", organizacao);

        // Assert
        resultado.Should().BeNull(); // Não deve encontrar com espaços
    }

    #endregion

    #region Testes EmailExisteAsync (sem exclusão)

    [Fact]
    public async Task EmailExisteAsync_DeveRetornarTrue_QuandoEmailExisteNaOrganizacao()
    {
        // Arrange
        var organizacao = IdOrganizacao.CriarNovo();
        var usuario = UsuarioTestHelper.CriarUsuarioComEmailELogin("teste@exemplo.com", "teste.usuario", organizacao);
        await _context.Usuarios.AddAsync(usuario);
        await _context.SaveChangesAsync();

        // Act
        var resultado = await _repositorio.EmailExisteAsync("teste@exemplo.com", organizacao);

        // Assert
        resultado.Should().BeTrue();
    }

    [Fact]
    public async Task EmailExisteAsync_DeveRetornarFalse_QuandoEmailNaoExiste()
    {
        // Arrange
        var organizacao = IdOrganizacao.CriarNovo();

        // Act
        var resultado = await _repositorio.EmailExisteAsync("inexistente@exemplo.com", organizacao);

        // Assert
        resultado.Should().BeFalse();
    }

    [Fact]
    public async Task EmailExisteAsync_DeveRetornarFalse_QuandoEmailExisteEmOutraOrganizacao()
    {
        // Arrange
        var organizacao1 = IdOrganizacao.CriarNovo();
        var organizacao2 = IdOrganizacao.CriarNovo();
        var usuario = UsuarioTestHelper.CriarUsuarioComEmailELogin("teste@exemplo.com", "teste.usuario", organizacao1);
        await _context.Usuarios.AddAsync(usuario);
        await _context.SaveChangesAsync();

        // Act
        var resultado = await _repositorio.EmailExisteAsync("teste@exemplo.com", organizacao2);

        // Assert
        resultado.Should().BeFalse();
    }

    [Fact]
    public async Task EmailExisteAsync_DeveSerCaseInsensitive()
    {
        // Arrange
        var organizacao = IdOrganizacao.CriarNovo();
        var usuario = UsuarioTestHelper.CriarUsuarioComEmailELogin("teste@exemplo.com", "teste.usuario", organizacao);
        await _context.Usuarios.AddAsync(usuario);
        await _context.SaveChangesAsync();

        // Act
        var resultado = await _repositorio.EmailExisteAsync("TESTE@EXEMPLO.COM", organizacao);

        // Assert
        resultado.Should().BeTrue();
    }

    #endregion

    #region Testes EmailExisteAsync (com exclusão)

    [Fact]
    public async Task EmailExisteAsync_ComExclusao_DeveRetornarFalse_QuandoEmailPertenceAoUsuarioExcluido()
    {
        // Arrange
        var organizacao = IdOrganizacao.CriarNovo();
        var usuario = UsuarioTestHelper.CriarUsuarioComEmailELogin("teste@exemplo.com", "teste.usuario", organizacao);
        await _context.Usuarios.AddAsync(usuario);
        await _context.SaveChangesAsync();

        // Act
        var resultado = await _repositorio.EmailExisteAsync("teste@exemplo.com", organizacao, usuario.Id);

        // Assert
        resultado.Should().BeFalse();
    }

    [Fact]
    public async Task EmailExisteAsync_ComExclusao_DeveRetornarTrue_QuandoEmailExisteEmOutroUsuario()
    {
        // Arrange
        var organizacao = IdOrganizacao.CriarNovo();
        var usuario1 = UsuarioTestHelper.CriarUsuarioComEmailELogin("teste@exemplo.com", "teste.usuario1", organizacao);
        var usuario2 = UsuarioTestHelper.CriarUsuarioComEmailELogin("outro@exemplo.com", "teste.usuario2", organizacao);
        await _context.Usuarios.AddRangeAsync(usuario1, usuario2);
        await _context.SaveChangesAsync();

        // Act
        var resultado = await _repositorio.EmailExisteAsync("teste@exemplo.com", organizacao, usuario2.Id);

        // Assert
        resultado.Should().BeTrue();
    }

    [Fact]
    public async Task EmailExisteAsync_ComExclusao_DeveRetornarFalse_QuandoEmailNaoExiste()
    {
        // Arrange
        var organizacao = IdOrganizacao.CriarNovo();
        var usuario = UsuarioTestHelper.CriarUsuarioComEmailELogin("teste@exemplo.com", "teste.usuario", organizacao);
        await _context.Usuarios.AddAsync(usuario);
        await _context.SaveChangesAsync();

        // Act
        var resultado = await _repositorio.EmailExisteAsync("inexistente@exemplo.com", organizacao, usuario.Id);

        // Assert
        resultado.Should().BeFalse();
    }

    [Fact]
    public async Task EmailExisteAsync_ComExclusao_DeveSerCaseInsensitive()
    {
        // Arrange
        var organizacao = IdOrganizacao.CriarNovo();
        var usuario1 = UsuarioTestHelper.CriarUsuarioComEmailELogin("teste@exemplo.com", "teste.usuario1", organizacao);
        var usuario2 = UsuarioTestHelper.CriarUsuarioComEmailELogin("outro@exemplo.com", "teste.usuario2", organizacao);
        await _context.Usuarios.AddRangeAsync(usuario1, usuario2);
        await _context.SaveChangesAsync();

        // Act
        var resultado = await _repositorio.EmailExisteAsync("TESTE@EXEMPLO.COM", organizacao, usuario2.Id);

        // Assert
        resultado.Should().BeTrue();
    }

    #endregion

    #region Testes LoginExisteAsync (sem exclusão)

    [Fact]
    public async Task LoginExisteAsync_DeveRetornarTrue_QuandoLoginExisteNaOrganizacao()
    {
        // Arrange
        var organizacao = IdOrganizacao.CriarNovo();
        var usuario = UsuarioTestHelper.CriarUsuarioComEmailELogin("teste@exemplo.com", "teste.usuario", organizacao);
        await _context.Usuarios.AddAsync(usuario);
        await _context.SaveChangesAsync();

        // Act
        var resultado = await _repositorio.LoginExisteAsync("teste.usuario", organizacao);

        // Assert
        resultado.Should().BeTrue();
    }

    [Fact]
    public async Task LoginExisteAsync_DeveRetornarFalse_QuandoLoginNaoExiste()
    {
        // Arrange
        var organizacao = IdOrganizacao.CriarNovo();

        // Act
        var resultado = await _repositorio.LoginExisteAsync("inexistente.usuario", organizacao);

        // Assert
        resultado.Should().BeFalse();
    }

    [Fact]
    public async Task LoginExisteAsync_DeveRetornarFalse_QuandoLoginExisteEmOutraOrganizacao()
    {
        // Arrange
        var organizacao1 = IdOrganizacao.CriarNovo();
        var organizacao2 = IdOrganizacao.CriarNovo();
        var usuario = UsuarioTestHelper.CriarUsuarioComEmailELogin("teste@exemplo.com", "teste.usuario", organizacao1);
        await _context.Usuarios.AddAsync(usuario);
        await _context.SaveChangesAsync();

        // Act
        var resultado = await _repositorio.LoginExisteAsync("teste.usuario", organizacao2);

        // Assert
        resultado.Should().BeFalse();
    }

    [Fact]
    public async Task LoginExisteAsync_DeveSerCaseInsensitive()
    {
        // Arrange
        var organizacao = IdOrganizacao.CriarNovo();
        var usuario = UsuarioTestHelper.CriarUsuarioComEmailELogin("teste@exemplo.com", "teste.usuario", organizacao);
        await _context.Usuarios.AddAsync(usuario);
        await _context.SaveChangesAsync();

        // Act
        var resultado = await _repositorio.LoginExisteAsync("TESTE.USUARIO", organizacao);

        // Assert
        resultado.Should().BeTrue();
    }

    #endregion

    #region Testes LoginExisteAsync (com exclusão)

    [Fact]
    public async Task LoginExisteAsync_ComExclusao_DeveRetornarFalse_QuandoLoginPertenceAoUsuarioExcluido()
    {
        // Arrange
        var organizacao = IdOrganizacao.CriarNovo();
        var usuario = UsuarioTestHelper.CriarUsuarioComEmailELogin("teste@exemplo.com", "teste.usuario", organizacao);
        await _context.Usuarios.AddAsync(usuario);
        await _context.SaveChangesAsync();

        // Act
        var resultado = await _repositorio.LoginExisteAsync("teste.usuario", organizacao, usuario.Id);

        // Assert
        resultado.Should().BeFalse();
    }

    [Fact]
    public async Task LoginExisteAsync_ComExclusao_DeveRetornarTrue_QuandoLoginExisteEmOutroUsuario()
    {
        // Arrange
        var organizacao = IdOrganizacao.CriarNovo();
        var usuario1 = UsuarioTestHelper.CriarUsuarioComEmailELogin("teste@exemplo.com", "teste.usuario1", organizacao);
        var usuario2 = UsuarioTestHelper.CriarUsuarioComEmailELogin("outro@exemplo.com", "teste.usuario2", organizacao);
        await _context.Usuarios.AddRangeAsync(usuario1, usuario2);
        await _context.SaveChangesAsync();

        // Act
        var resultado = await _repositorio.LoginExisteAsync("teste.usuario1", organizacao, usuario2.Id);

        // Assert
        resultado.Should().BeTrue();
    }

    [Fact]
    public async Task LoginExisteAsync_ComExclusao_DeveRetornarFalse_QuandoLoginNaoExiste()
    {
        // Arrange
        var organizacao = IdOrganizacao.CriarNovo();
        var usuario = UsuarioTestHelper.CriarUsuarioComEmailELogin("teste@exemplo.com", "teste.usuario", organizacao);
        await _context.Usuarios.AddAsync(usuario);
        await _context.SaveChangesAsync();

        // Act
        var resultado = await _repositorio.LoginExisteAsync("inexistente.usuario", organizacao, usuario.Id);

        // Assert
        resultado.Should().BeFalse();
    }

    [Fact]
    public async Task LoginExisteAsync_ComExclusao_DeveSerCaseInsensitive()
    {
        // Arrange
        var organizacao = IdOrganizacao.CriarNovo();
        var usuario1 = UsuarioTestHelper.CriarUsuarioComEmailELogin("teste@exemplo.com", "teste.usuario1", organizacao);
        var usuario2 = UsuarioTestHelper.CriarUsuarioComEmailELogin("outro@exemplo.com", "teste.usuario2", organizacao);
        await _context.Usuarios.AddRangeAsync(usuario1, usuario2);
        await _context.SaveChangesAsync();

        // Act
        var resultado = await _repositorio.LoginExisteAsync("TESTE.USUARIO1", organizacao, usuario2.Id);

        // Assert
        resultado.Should().BeTrue();
    }

    #endregion

    #region Testes ObterPorPerfilAsync

    [Fact]
    public async Task ObterPorPerfilAsync_DeveRetornarUsuariosComPerfilEspecifico()
    {
        // Arrange
        var organizacao = IdOrganizacao.CriarNovo();
        var usuarioAdmin = UsuarioTestHelper.CriarUsuarioComPerfil(PerfilUsuario.Administrador, organizacao);
        var usuarioComum1 = UsuarioTestHelper.CriarUsuarioComPerfil(PerfilUsuario.Usuario, organizacao);
        var usuarioComum2 = UsuarioTestHelper.CriarUsuarioComPerfil(PerfilUsuario.Usuario, organizacao);
        
        await _context.Usuarios.AddRangeAsync(usuarioAdmin, usuarioComum1, usuarioComum2);
        await _context.SaveChangesAsync();

        // Act
        var resultado = await _repositorio.ObterPorPerfilAsync((int)PerfilUsuario.Usuario, organizacao);

        // Assert
        resultado.Should().HaveCount(2);
        resultado.Should().AllSatisfy(u => u.Perfil.Should().Be(PerfilUsuario.Usuario));
        resultado.Should().AllSatisfy(u => u.IdOrganizacao.Should().Be(organizacao));
    }

    [Fact]
    public async Task ObterPorPerfilAsync_DeveRetornarListaVazia_QuandoNaoHaUsuariosComPerfil()
    {
        // Arrange
        var organizacao = IdOrganizacao.CriarNovo();
        var usuarioComum = UsuarioTestHelper.CriarUsuarioComPerfil(PerfilUsuario.Usuario, organizacao);
        await _context.Usuarios.AddAsync(usuarioComum);
        await _context.SaveChangesAsync();

        // Act
        var resultado = await _repositorio.ObterPorPerfilAsync((int)PerfilUsuario.Administrador, organizacao);

        // Assert
        resultado.Should().BeEmpty();
    }

    [Fact]
    public async Task ObterPorPerfilAsync_DeveFiltrarPorOrganizacao()
    {
        // Arrange
        var organizacao1 = IdOrganizacao.CriarNovo();
        var organizacao2 = IdOrganizacao.CriarNovo();
        var usuarioOrg1 = UsuarioTestHelper.CriarUsuarioComPerfil(PerfilUsuario.Usuario, organizacao1);
        var usuarioOrg2 = UsuarioTestHelper.CriarUsuarioComPerfil(PerfilUsuario.Usuario, organizacao2);
        
        await _context.Usuarios.AddRangeAsync(usuarioOrg1, usuarioOrg2);
        await _context.SaveChangesAsync();

        // Act
        var resultado = await _repositorio.ObterPorPerfilAsync((int)PerfilUsuario.Usuario, organizacao1);

        // Assert
        resultado.Should().HaveCount(1);
        resultado.First().IdOrganizacao.Should().Be(organizacao1);
    }

    [Fact]
    public async Task ObterPorPerfilAsync_DeveRetornarMultiplosUsuarios()
    {
        // Arrange
        var organizacao = IdOrganizacao.CriarNovo();
        var usuarios = new List<Usuario>();
        for (int i = 0; i < 5; i++)
        {
            usuarios.Add(UsuarioTestHelper.CriarUsuarioComPerfil(PerfilUsuario.Usuario, organizacao));
        }
        
        await _context.Usuarios.AddRangeAsync(usuarios);
        await _context.SaveChangesAsync();

        // Act
        var resultado = await _repositorio.ObterPorPerfilAsync((int)PerfilUsuario.Usuario, organizacao);

        // Assert
        resultado.Should().HaveCount(5);
        resultado.Should().AllSatisfy(u => u.Perfil.Should().Be(PerfilUsuario.Usuario));
    }

    #endregion

    #region Testes de Herança - RepositorioBaseComOrganizacao

    [Fact]
    public async Task AdicionarAsync_DeveAdicionarUsuarioComSucesso()
    {
        // Arrange
        var usuario = UsuarioTestHelper.CriarUsuarioValido();

        // Act
        var resultado = await _repositorio.AdicionarAsync(usuario);
        await _context.SaveChangesAsync();

        // Assert
        resultado.Should().NotBeNull();
        resultado.Id.Should().Be(usuario.Id);
        
        var usuarioSalvo = await _context.Usuarios.FindAsync(usuario.Id);
        usuarioSalvo.Should().NotBeNull();
    }

    [Fact]
    public async Task AtualizarAsync_DeveAtualizarUsuarioExistente()
    {
        // Arrange
        var usuario = UsuarioTestHelper.CriarUsuarioValido();
        await _context.Usuarios.AddAsync(usuario);
        await _context.SaveChangesAsync();
        
        var usuarioModificacao = IdUsuario.GerarNovo();
        usuario.AlterarStatus(StatusUsuario.Inativo, usuarioModificacao);

        // Act
        var resultado = await _repositorio.AtualizarAsync(usuario);
        await _context.SaveChangesAsync();

        // Assert
        resultado.Status.Should().Be(StatusUsuario.Inativo);
        
        var usuarioSalvo = await _context.Usuarios.FindAsync(usuario.Id);
        usuarioSalvo!.Status.Should().Be(StatusUsuario.Inativo);
    }

    [Fact]
    public async Task ObterPorIdETOrganizacaoAsync_DeveRetornarUsuario_QuandoExisteNaOrganizacao()
    {
        // Arrange
        var organizacao = IdOrganizacao.CriarNovo();
        var usuario = UsuarioTestHelper.CriarUsuarioComOrganizacao(organizacao);
        await _context.Usuarios.AddAsync(usuario);
        await _context.SaveChangesAsync();

        // Act
        var resultado = await _repositorio.ObterPorIdETOrganizacaoAsync(usuario.Id, organizacao);

        // Assert
        resultado.Should().NotBeNull();
        resultado!.Id.Should().Be(usuario.Id);
        resultado.IdOrganizacao.Should().Be(organizacao);
    }

    [Fact]
    public async Task ObterPorIdETOrganizacaoAsync_DeveRetornarNull_QuandoUsuarioEDeOutraOrganizacao()
    {
        // Arrange
        var organizacao1 = IdOrganizacao.CriarNovo();
        var organizacao2 = IdOrganizacao.CriarNovo();
        var usuario = UsuarioTestHelper.CriarUsuarioComOrganizacao(organizacao1);
        await _context.Usuarios.AddAsync(usuario);
        await _context.SaveChangesAsync();

        // Act
        var resultado = await _repositorio.ObterPorIdETOrganizacaoAsync(usuario.Id, organizacao2);

        // Assert
        resultado.Should().BeNull();
    }

    [Fact]
    public async Task ObterPorTenanteAsync_DeveRetornarUsuariosDaOrganizacao()
    {
        // Arrange
        var organizacao1 = IdOrganizacao.CriarNovo();
        var organizacao2 = IdOrganizacao.CriarNovo();
        var usuariosOrg1 = UsuarioTestHelper.CriarListaUsuarios(3, organizacao1);
        var usuariosOrg2 = UsuarioTestHelper.CriarListaUsuarios(2, organizacao2);
        
        await _context.Usuarios.AddRangeAsync(usuariosOrg1);
        await _context.Usuarios.AddRangeAsync(usuariosOrg2);
        await _context.SaveChangesAsync();

        // Act
        var resultado = await _repositorio.ObterPorTenanteAsync(organizacao1);

        // Assert
        resultado.Should().HaveCount(3);
        resultado.Should().AllSatisfy(u => u.IdOrganizacao.Should().Be(organizacao1));
    }

    [Fact]
    public async Task RemoverAsync_DeveRemoverUsuario()
    {
        // Arrange
        var usuario = UsuarioTestHelper.CriarUsuarioValido();
        await _context.Usuarios.AddAsync(usuario);
        await _context.SaveChangesAsync();

        // Act
        var resultado = await _repositorio.RemoverAsync(usuario);
        await _context.SaveChangesAsync();

        // Assert
        resultado.Should().BeTrue();
        
        var usuarioRemovido = await _context.Usuarios.FindAsync(usuario.Id);
        usuarioRemovido.Should().BeNull();
    }

    #endregion

    #region Testes de Cenários Especiais

    [Fact]
    public async Task ObterPorEmailAsync_DeveTratarCancellationToken()
    {
        // Arrange
        var organizacao = IdOrganizacao.CriarNovo();
        var usuario = UsuarioTestHelper.CriarUsuarioComEmailELogin("teste@exemplo.com", "teste.usuario", organizacao);
        await _context.Usuarios.AddAsync(usuario);
        await _context.SaveChangesAsync();

        using var cts = new CancellationTokenSource();
        await cts.CancelAsync();

        // Act & Assert
        await Assert.ThrowsAsync<OperationCanceledException>(() =>
            _repositorio.ObterPorEmailAsync("teste@exemplo.com", organizacao, cts.Token));
    }

    [Fact]
    public async Task ObterPorEmailAsync_DeveTratarCaracteresEspeciais()
    {
        // Arrange
        var organizacao = IdOrganizacao.CriarNovo();
        var emailEspecial = "teste+especial@exemplo.com";
        var usuario = UsuarioTestHelper.CriarUsuarioComEmailELogin(emailEspecial, "teste.usuario", organizacao);
        await _context.Usuarios.AddAsync(usuario);
        await _context.SaveChangesAsync();

        // Act
        var resultado = await _repositorio.ObterPorEmailAsync(emailEspecial, organizacao);

        // Assert
        resultado.Should().NotBeNull();
        resultado!.Email.Should().Be(emailEspecial);
    }

    [Fact]
    public async Task ObterPorPerfilAsync_DeveTratarGrandeVolumeDeUsuarios()
    {
        // Arrange
        var organizacao = IdOrganizacao.CriarNovo();
        var usuarios = new List<Usuario>();
        
        // Criar 100 usuários com perfil comum
        for (int i = 0; i < 100; i++)
        {
            usuarios.Add(UsuarioTestHelper.CriarUsuarioComPerfil(PerfilUsuario.Usuario, organizacao));
        }
        
        // Criar 50 usuários administradores
        for (int i = 0; i < 50; i++)
        {
            usuarios.Add(UsuarioTestHelper.CriarUsuarioComPerfil(PerfilUsuario.Administrador, organizacao));
        }
        
        await _context.Usuarios.AddRangeAsync(usuarios);
        await _context.SaveChangesAsync();

        // Act
        var usuariosComuns = await _repositorio.ObterPorPerfilAsync((int)PerfilUsuario.Usuario, organizacao);
        var usuariosAdmin = await _repositorio.ObterPorPerfilAsync((int)PerfilUsuario.Administrador, organizacao);

        // Assert
        usuariosComuns.Should().HaveCount(100);
        usuariosAdmin.Should().HaveCount(50);
    }

    #endregion
}
