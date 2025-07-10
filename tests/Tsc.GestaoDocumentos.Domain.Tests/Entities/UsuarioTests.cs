using FluentAssertions;
using Tsc.GestaoDocumentos.Domain.Entities;
using Tsc.GestaoDocumentos.Domain.Enums;
using Xunit;

namespace Tsc.GestaoDocumentos.Domain.Tests.Entities;

public class UsuarioTests
{
    [Fact]
    public void Usuario_DeveCriarComDadosValidos()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var nome = "João Silva";
        var email = "joao@exemplo.com";
        var login = "joao.silva";
        var senhaHash = "hash_da_senha";
        var perfil = PerfilUsuario.Usuario;
        var usuarioCriacao = Guid.NewGuid();

        // Act
        var usuario = new Usuario(tenantId, nome, email, login, senhaHash, perfil, usuarioCriacao);

        // Assert
        usuario.TenantId.Should().Be(tenantId);
        usuario.Nome.Should().Be(nome);
        usuario.Email.Should().Be(email.ToLowerInvariant());
        usuario.Login.Should().Be(login.ToLowerInvariant());
        usuario.SenhaHash.Should().Be(senhaHash);
        usuario.Perfil.Should().Be(perfil);
        usuario.Status.Should().Be(StatusUsuario.Ativo);
        usuario.UsuarioCriacao.Should().Be(usuarioCriacao);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Usuario_DeveRejeitarNomeInvalido(string? nomeInvalido)
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var email = "joao@exemplo.com";
        var login = "joao.silva";
        var senhaHash = "hash_da_senha";
        var perfil = PerfilUsuario.Usuario;
        var usuarioCriacao = Guid.NewGuid();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => 
            new Usuario(tenantId, nomeInvalido ?? "", email, login, senhaHash, perfil, usuarioCriacao));
    }

    [Theory]
    [InlineData("email_invalido")]
    [InlineData("@exemplo.com")]
    [InlineData("joao@")]
    [InlineData("")]
    public void Usuario_DeveRejeitarEmailInvalido(string emailInvalido)
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var nome = "João Silva";
        var login = "joao.silva";
        var senhaHash = "hash_da_senha";
        var perfil = PerfilUsuario.Usuario;
        var usuarioCriacao = Guid.NewGuid();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => 
            new Usuario(tenantId, nome, emailInvalido, login, senhaHash, perfil, usuarioCriacao));
    }

    [Fact]
    public void Usuario_DeveNormalizarEmailParaMinusculo()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var nome = "João Silva";
        var email = "JOAO@EXEMPLO.COM";
        var login = "joao.silva";
        var senhaHash = "hash_da_senha";
        var perfil = PerfilUsuario.Usuario;
        var usuarioCriacao = Guid.NewGuid();

        // Act
        var usuario = new Usuario(tenantId, nome, email, login, senhaHash, perfil, usuarioCriacao);

        // Assert
        usuario.Email.Should().Be("joao@exemplo.com");
    }

    [Fact]
    public void Usuario_DeveNormalizarLoginParaMinusculo()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var nome = "João Silva";
        var email = "joao@exemplo.com";
        var login = "JOAO.SILVA";
        var senhaHash = "hash_da_senha";
        var perfil = PerfilUsuario.Usuario;
        var usuarioCriacao = Guid.NewGuid();

        // Act
        var usuario = new Usuario(tenantId, nome, email, login, senhaHash, perfil, usuarioCriacao);

        // Assert
        usuario.Login.Should().Be("joao.silva");
    }

    [Fact]
    public void Usuario_DeveAlterarStatus()
    {
        // Arrange
        var usuario = CriarUsuarioValido();
        var usuarioAlteracao = Guid.NewGuid();
        var novoStatus = StatusUsuario.Inativo;

        // Act
        usuario.AlterarStatus(novoStatus, usuarioAlteracao);

        // Assert
        usuario.Status.Should().Be(novoStatus);
        usuario.UsuarioUltimaAlteracao.Should().Be(usuarioAlteracao);
    }

    [Fact]
    public void Usuario_DeveAlterarPerfil()
    {
        // Arrange
        var usuario = CriarUsuarioValido();
        var usuarioAlteracao = Guid.NewGuid();
        var novoPerfil = PerfilUsuario.Administrador;

        // Act
        usuario.AlterarPerfil(novoPerfil, usuarioAlteracao);

        // Assert
        usuario.Perfil.Should().Be(novoPerfil);
        usuario.UsuarioUltimaAlteracao.Should().Be(usuarioAlteracao);
    }

    [Fact]
    public void Usuario_DeveRegistrarAcesso()
    {
        // Arrange
        var usuario = CriarUsuarioValido();
        var dataAntes = DateTime.UtcNow;

        // Act
        usuario.RegistrarAcesso();

        // Assert
        usuario.UltimoAcesso.Should().NotBeNull();
        usuario.UltimoAcesso.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        usuario.UltimoAcesso.Should().BeOnOrAfter(dataAntes);
    }

    [Fact]
    public void Usuario_DeveEstarAtivo()
    {
        // Arrange
        var usuario = CriarUsuarioValido();

        // Act & Assert
        usuario.EstaAtivo().Should().BeTrue();
    }

    private static Usuario CriarUsuarioValido()
    {
        return new Usuario(
            Guid.NewGuid(),
            "João Silva",
            "joao@exemplo.com",
            "joao.silva",
            "hash_da_senha",
            PerfilUsuario.Usuario,
            Guid.NewGuid());
    }
}
