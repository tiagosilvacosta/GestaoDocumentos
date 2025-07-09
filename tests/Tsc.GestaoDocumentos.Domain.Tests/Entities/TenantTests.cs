using FluentAssertions;
using Tsc.GestaoDocumentos.Domain.Entities;
using Tsc.GestaoDocumentos.Domain.Enums;

namespace Tsc.GestaoDocumentos.Domain.Tests.Entities;

public class TenantTests
{
    [Fact]
    public void Tenant_DeveCriarComDadosValidos()
    {
        // Arrange
        var nomeOrganizacao = "Organização Teste";
        var slug = "organizacao-teste";
        var usuarioCriacao = Guid.NewGuid();

        // Act
        var tenant = new Tenant(nomeOrganizacao, slug, usuarioCriacao);

        // Assert
        tenant.NomeOrganizacao.Should().Be(nomeOrganizacao);
        tenant.Slug.Should().Be(slug);
        tenant.Status.Should().Be(StatusTenant.Ativo);
        tenant.UsuarioCriacao.Should().Be(usuarioCriacao);
        tenant.Id.Should().NotBeEmpty();
        tenant.DataCriacao.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Tenant_DeveRejeitarNomeOrganizacaoInvalido(string nomeInvalido)
    {
        // Arrange
        var slug = "organizacao-teste";
        var usuarioCriacao = Guid.NewGuid();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Tenant(nomeInvalido, slug, usuarioCriacao));
    }

    [Theory]
    [InlineData("SLUG_MAIUSCULO")]
    [InlineData("slug com espaços")]
    [InlineData("slug@especial")]
    public void Tenant_DeveRejeitarSlugInvalido(string slugInvalido)
    {
        // Arrange
        var nomeOrganizacao = "Organização Teste";
        var usuarioCriacao = Guid.NewGuid();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Tenant(nomeOrganizacao, slugInvalido, usuarioCriacao));
    }

    [Fact]
    public void Tenant_DevePermitirSlugValido()
    {
        // Arrange
        var nomeOrganizacao = "Organização Teste";
        var slug = "organizacao-teste-123";
        var usuarioCriacao = Guid.NewGuid();

        // Act
        var tenant = new Tenant(nomeOrganizacao, slug, usuarioCriacao);

        // Assert
        tenant.Slug.Should().Be(slug);
    }

    [Fact]
    public void Tenant_DeveAlterarStatus()
    {
        // Arrange
        var tenant = new Tenant("Organização", "organizacao", Guid.NewGuid());
        var usuarioAlteracao = Guid.NewGuid();
        var novoStatus = StatusTenant.Suspenso;

        // Act
        tenant.AlterarStatus(novoStatus, usuarioAlteracao);

        // Assert
        tenant.Status.Should().Be(novoStatus);
        tenant.UsuarioUltimaAlteracao.Should().Be(usuarioAlteracao);
    }

    [Fact]
    public void Tenant_DeveValidarDataExpiracao()
    {
        // Arrange
        var tenant = new Tenant("Organização", "organizacao", Guid.NewGuid());
        var usuarioAlteracao = Guid.NewGuid();
        var dataExpiracaoPassada = DateTime.UtcNow.AddDays(-1);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => 
            tenant.DefinirDataExpiracao(dataExpiracaoPassada, usuarioAlteracao));
    }

    [Fact]
    public void Tenant_DeveEstarAtivo()
    {
        // Arrange
        var tenant = new Tenant("Organização", "organizacao", Guid.NewGuid());

        // Act & Assert
        tenant.EstaAtivo().Should().BeTrue();
    }

    [Fact]
    public void Tenant_DeveValidarSeEstaExpirado()
    {
        // Arrange
        var tenant = new Tenant("Organização", "organizacao", Guid.NewGuid());
        var usuarioAlteracao = Guid.NewGuid();
        var dataExpiracao = DateTime.UtcNow.AddDays(1);

        // Act
        tenant.DefinirDataExpiracao(dataExpiracao, usuarioAlteracao);

        // Assert
        tenant.EstaExpirado().Should().BeFalse();
    }
}
