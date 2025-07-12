using FluentAssertions;
using Tsc.GestaoDocumentos.Application.Tests.Mappings.Helpers;
using Tsc.GestaoDocumentos.Application.Usuarios;
using Tsc.GestaoDocumentos.Domain.Usuarios;
using Xunit;

namespace Tsc.GestaoDocumentos.Application.Tests.Mappings;

/// <summary>
/// Testes para validar o mapeamento de Usuario para UsuarioDto
/// </summary>
public class UsuarioMappingTests : MappingTestsBase
{
    [Fact]
    public void DeveMappearUsuarioParaUsuarioDto_ComSucesso()
    {
        // Arrange
        var idOrganizacao = CriarIdOrganizacao();
        var usuarioCriacao = CriarIdUsuario();
        var ultimoAcesso = DateTime.UtcNow.AddHours(-2);
        
        var usuario = TestDataBuilders.CriarUsuario(
            idOrganizacao: idOrganizacao,
            nome: "João Silva Santos",
            email: "joao.silva@empresa.com",
            login: "joao.silva",
            perfil: PerfilUsuario.Administrador,
            status: StatusUsuario.Ativo,
            ultimoAcesso: ultimoAcesso,
            usuarioCriacao: usuarioCriacao
        );

        // Act
        var dto = Mapper.Map<UsuarioDto>(usuario);

        // Assert
        dto.Should().NotBeNull();
        dto.Id.Should().Be(usuario.Id.Valor);
        dto.IdOrganizacao.Should().Be(usuario.IdOrganizacao);
        dto.Nome.Should().Be(usuario.Nome);
        dto.Email.Should().Be(usuario.Email);
        dto.Login.Should().Be(usuario.Login);
        dto.Status.Should().Be(usuario.Status.ToString());
        dto.Perfil.Should().Be(usuario.Perfil.ToString());
        dto.UltimoAcesso.Should().Be(usuario.UltimoAcesso);
        dto.DataCriacao.Should().Be(usuario.DataCriacao);
        dto.DataUltimaAlteracao.Should().Be(usuario.DataAtualizacao);
        dto.UsuarioCriacao.Should().Be(usuario.UsuarioCriacao.Valor);
        dto.UsuarioUltimaAlteracao.Should().Be(usuario.UsuarioUltimaAlteracao.Valor);
    }

    [Fact]
    public void DeveMappearUsuarioComPerfilUsuario_ComSucesso()
    {
        // Arrange
        var usuario = TestDataBuilders.CriarUsuario(
            perfil: PerfilUsuario.Usuario
        );

        // Act
        var dto = Mapper.Map<UsuarioDto>(usuario);

        // Assert
        dto.Should().NotBeNull();
        dto.Perfil.Should().Be("Usuario");
    }

    [Fact]
    public void DeveMappearUsuarioComPerfilAdministrador_ComSucesso()
    {
        // Arrange
        var usuario = TestDataBuilders.CriarUsuario(
            perfil: PerfilUsuario.Administrador
        );

        // Act
        var dto = Mapper.Map<UsuarioDto>(usuario);

        // Assert
        dto.Should().NotBeNull();
        dto.Perfil.Should().Be("Administrador");
    }

    [Fact]
    public void DeveMappearUsuarioComStatusInativo_ComSucesso()
    {
        // Arrange
        var usuarioAlteracao = CriarIdUsuario();
        var usuario = TestDataBuilders.CriarUsuario(
            status: StatusUsuario.Inativo
        );

        // Act
        var dto = Mapper.Map<UsuarioDto>(usuario);

        // Assert
        dto.Should().NotBeNull();
        dto.Status.Should().Be("Inativo");
    }

    [Fact]
    public void DeveMappearUsuarioSemUltimoAcesso_ComSucesso()
    {
        // Arrange
        var usuario = TestDataBuilders.CriarUsuario(
            ultimoAcesso: null
        );

        // Act
        var dto = Mapper.Map<UsuarioDto>(usuario);

        // Assert
        dto.Should().NotBeNull();
        dto.UltimoAcesso.Should().BeNull();
    }

    [Fact]
    public void DeveMappearIdUsuarioCorretamente()
    {
        // Arrange
        var idEspecifico = CriarIdUsuario();
        var usuario = TestDataBuilders.CriarUsuario(id: idEspecifico);

        // Act
        var dto = Mapper.Map<UsuarioDto>(usuario);

        // Assert
        dto.Should().NotBeNull();
        dto.Id.Should().Be(idEspecifico.Valor);
    }

    [Fact]
    public void DeveMappearIdOrganizacaoCorretamente()
    {
        // Arrange
        var idOrganizacaoEspecifico = CriarIdOrganizacao();
        var usuario = TestDataBuilders.CriarUsuario(idOrganizacao: idOrganizacaoEspecifico);

        // Act
        var dto = Mapper.Map<UsuarioDto>(usuario);

        // Assert
        dto.Should().NotBeNull();
        dto.IdOrganizacao.Should().Be(idOrganizacaoEspecifico);
    }

    [Fact]
    public void DeveMappearEmailCorretamente()
    {
        // Arrange
        var emailEspecifico = "usuario.teste@empresa.com.br";
        var usuario = TestDataBuilders.CriarUsuario(email: emailEspecifico);

        // Act
        var dto = Mapper.Map<UsuarioDto>(usuario);

        // Assert
        dto.Should().NotBeNull();
        dto.Email.Should().Be(emailEspecifico);
    }

    [Fact]
    public void DeveMappearLoginCorretamente()
    {
        // Arrange
        var loginEspecifico = "usuario.sistema";
        var usuario = TestDataBuilders.CriarUsuario(login: loginEspecifico);

        // Act
        var dto = Mapper.Map<UsuarioDto>(usuario);

        // Assert
        dto.Should().NotBeNull();
        dto.Login.Should().Be(loginEspecifico);
    }

    [Fact]
    public void DeveMappearNomeCorretamente()
    {
        // Arrange
        var nomeEspecifico = "Maria da Silva Oliveira";
        var usuario = TestDataBuilders.CriarUsuario(nome: nomeEspecifico);

        // Act
        var dto = Mapper.Map<UsuarioDto>(usuario);

        // Assert
        dto.Should().NotBeNull();
        dto.Nome.Should().Be(nomeEspecifico);
    }

    [Fact]
    public void DeveMappearCamposDeAuditoriaCorretamente()
    {
        // Arrange
        var usuarioCriacao = CriarIdUsuario();
        var usuarioAlteracao = CriarIdUsuario();
        
        var usuario = TestDataBuilders.CriarUsuario(
            usuarioCriacao: usuarioCriacao,
            usuarioUltimaAlteracao: usuarioAlteracao
        );

        // Act
        var dto = Mapper.Map<UsuarioDto>(usuario);

        // Assert
        dto.Should().NotBeNull();
        dto.UsuarioCriacao.Should().Be(usuarioCriacao.Valor);
        dto.UsuarioUltimaAlteracao.Should().Be(usuarioAlteracao.Valor);
    }

    [Fact]
    public void DeveMappearListaDeUsuarios_ComSucesso()
    {
        // Arrange
        var usuarios = new List<Usuario>
        {
            TestDataBuilders.CriarUsuario(nome: "Usuário 1"),
            TestDataBuilders.CriarUsuario(nome: "Usuário 2"),
            TestDataBuilders.CriarUsuario(nome: "Usuário 3")
        };

        // Act
        var dtos = Mapper.Map<List<UsuarioDto>>(usuarios);

        // Assert
        dtos.Should().NotBeNull();
        dtos.Should().HaveCount(3);
        dtos[0].Nome.Should().Be("Usuário 1");
        dtos[1].Nome.Should().Be("Usuário 2");
        dtos[2].Nome.Should().Be("Usuário 3");
    }

    [Theory]
    [InlineData(StatusUsuario.Ativo, "Ativo")]
    [InlineData(StatusUsuario.Inativo, "Inativo")]
    public void DeveMappearTodosOsStatusCorretamente(StatusUsuario status, string statusEsperado)
    {
        // Arrange
        var usuario = TestDataBuilders.CriarUsuario(status: status);

        // Act
        var dto = Mapper.Map<UsuarioDto>(usuario);

        // Assert
        dto.Should().NotBeNull();
        dto.Status.Should().Be(statusEsperado);
    }

    [Theory]
    [InlineData(PerfilUsuario.Usuario, "Usuario")]
    [InlineData(PerfilUsuario.Administrador, "Administrador")]
    public void DeveMappearTodosOsPerfisCorretamente(PerfilUsuario perfil, string perfilEsperado)
    {
        // Arrange
        var usuario = TestDataBuilders.CriarUsuario(perfil: perfil);

        // Act
        var dto = Mapper.Map<UsuarioDto>(usuario);

        // Assert
        dto.Should().NotBeNull();
        dto.Perfil.Should().Be(perfilEsperado);
    }

    [Fact]
    public void NaoDeveMappearSenhaHash()
    {
        // Arrange
        var senhaHashEspecifica = "hash_muito_seguro_abc123";
        var usuario = TestDataBuilders.CriarUsuario(senhaHash: senhaHashEspecifica);

        // Act
        var dto = Mapper.Map<UsuarioDto>(usuario);

        // Assert
        dto.Should().NotBeNull();
        // UsuarioDto não deve ter propriedade SenhaHash para segurança
        typeof(UsuarioDto).GetProperty("SenhaHash").Should().BeNull();
    }
}