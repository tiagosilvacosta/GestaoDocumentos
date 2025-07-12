using FluentAssertions;
using Tsc.GestaoDocumentos.Application.Organizacoes;
using Tsc.GestaoDocumentos.Application.Tests.Mappings.Helpers;
using Tsc.GestaoDocumentos.Domain.Organizacoes;
using Xunit;

namespace Tsc.GestaoDocumentos.Application.Tests.Mappings;

/// <summary>
/// Testes para validar o mapeamento de Organizacao para OrganizacaoDto
/// </summary>
public class OrganizacaoMappingTests : MappingTestsBase
{
    [Fact]
    public void DeveMappearOrganizacaoParaOrganizacaoDto_ComSucesso()
    {
        // Arrange
        var usuarioCriacao = CriarIdUsuario();
        var usuarioAlteracao = CriarIdUsuario();
        var dataExpiracao = DateTime.UtcNow.AddMonths(12);
        
        var organizacao = TestDataBuilders.CriarOrganizacao(
            nomeOrganizacao: "Empresa Teste Ltda",
            slug: "empresa-teste",
            status: StatusTenant.Ativo,
            dataExpiracao: dataExpiracao,
            usuarioCriacao: usuarioCriacao,
            usuarioUltimaAlteracao: usuarioAlteracao
        );

        // Act
        var dto = Mapper.Map<OrganizacaoDto>(organizacao);

        // Assert
        dto.Should().NotBeNull();
        dto.Id.Should().Be(organizacao.Id.Valor);
        dto.NomeOrganizacao.Should().Be(organizacao.NomeOrganizacao);
        dto.Slug.Should().Be(organizacao.Slug);
        dto.Status.Should().Be(organizacao.Status.ToString());
        dto.DataExpiracao.Should().Be(organizacao.DataExpiracao);
        dto.DataCriacao.Should().Be(organizacao.DataCriacao);
        dto.DataUltimaAlteracao.Should().Be(organizacao.DataAtualizacao);
        dto.UsuarioCriacao.Should().Be(organizacao.UsuarioCriacao.Valor);
        dto.UsuarioUltimaAlteracao.Should().Be(organizacao.UsuarioUltimaAlteracao.Valor);
    }

    [Fact]
    public void DeveMappearOrganizacaoComStatusInativo_ComSucesso()
    {
        // Arrange
        var usuarioAlteracao = CriarIdUsuario();
        var organizacao = TestDataBuilders.CriarOrganizacao(
            status: StatusTenant.Inativo,
            usuarioUltimaAlteracao: usuarioAlteracao
        );

        // Act
        var dto = Mapper.Map<OrganizacaoDto>(organizacao);

        // Assert
        dto.Should().NotBeNull();
        dto.Status.Should().Be("Inativo");
    }

    [Fact]
    public void DeveMappearOrganizacaoComStatusSuspenso_ComSucesso()
    {
        // Arrange
        var usuarioAlteracao = CriarIdUsuario();
        var organizacao = TestDataBuilders.CriarOrganizacao(
            status: StatusTenant.Suspenso,
            usuarioUltimaAlteracao: usuarioAlteracao
        );

        // Act
        var dto = Mapper.Map<OrganizacaoDto>(organizacao);

        // Assert
        dto.Should().NotBeNull();
        dto.Status.Should().Be("Suspenso");
    }

    [Fact]
    public void DeveMappearOrganizacaoSemDataExpiracao_ComSucesso()
    {
        // Arrange
        var organizacao = TestDataBuilders.CriarOrganizacao(
            dataExpiracao: null
        );

        // Act
        var dto = Mapper.Map<OrganizacaoDto>(organizacao);

        // Assert
        dto.Should().NotBeNull();
        dto.DataExpiracao.Should().BeNull();
    }

    [Fact]
    public void DeveMappearIdOrganizacaoCorretamente()
    {
        // Arrange
        var idEspecifico = IdOrganizacao.CriarNovo();
        var organizacao = TestDataBuilders.CriarOrganizacao(id: idEspecifico);

        // Act
        var dto = Mapper.Map<OrganizacaoDto>(organizacao);

        // Assert
        dto.Should().NotBeNull();
        dto.Id.Should().Be(idEspecifico.Valor);
    }

    [Fact]
    public void DeveMappearCamposDeAuditoriaCorretamente()
    {
        // Arrange
        var usuarioCriacao = CriarIdUsuario();
        var usuarioAlteracao = CriarIdUsuario();
        
        var organizacao = TestDataBuilders.CriarOrganizacao(
            usuarioCriacao: usuarioCriacao,
            usuarioUltimaAlteracao: usuarioAlteracao
        );

        // Act
        var dto = Mapper.Map<OrganizacaoDto>(organizacao);

        // Assert
        dto.Should().NotBeNull();
        dto.UsuarioCriacao.Should().Be(usuarioCriacao.Valor);
        dto.UsuarioUltimaAlteracao.Should().Be(usuarioAlteracao.Valor);
        dto.DataCriacao.Should().Be(organizacao.DataCriacao);
        dto.DataUltimaAlteracao.Should().Be(organizacao.DataAtualizacao);
    }

    [Fact]
    public void DeveMappearSlugCorretamente()
    {
        // Arrange
        var slugEspecifico = "minha-empresa-especial";
        var organizacao = TestDataBuilders.CriarOrganizacao(slug: slugEspecifico);

        // Act
        var dto = Mapper.Map<OrganizacaoDto>(organizacao);

        // Assert
        dto.Should().NotBeNull();
        dto.Slug.Should().Be(slugEspecifico);
    }

    [Fact]
    public void DeveMappearNomeOrganizacaoCorretamente()
    {
        // Arrange
        var nomeEspecifico = "Empresa de Tecnologia Avançada S.A.";
        var organizacao = TestDataBuilders.CriarOrganizacao(nomeOrganizacao: nomeEspecifico);

        // Act
        var dto = Mapper.Map<OrganizacaoDto>(organizacao);

        // Assert
        dto.Should().NotBeNull();
        dto.NomeOrganizacao.Should().Be(nomeEspecifico);
    }

    [Fact]
    public void DeveMappearListaDeOrganizacoes_ComSucesso()
    {
        // Arrange
        var organizacoes = new List<Organizacao>
        {
            TestDataBuilders.CriarOrganizacao(nomeOrganizacao: "Empresa 1"),
            TestDataBuilders.CriarOrganizacao(nomeOrganizacao: "Empresa 2"),
            TestDataBuilders.CriarOrganizacao(nomeOrganizacao: "Empresa 3")
        };

        // Act
        var dtos = Mapper.Map<List<OrganizacaoDto>>(organizacoes);

        // Assert
        dtos.Should().NotBeNull();
        dtos.Should().HaveCount(3);
        dtos[0].NomeOrganizacao.Should().Be("Empresa 1");
        dtos[1].NomeOrganizacao.Should().Be("Empresa 2");
        dtos[2].NomeOrganizacao.Should().Be("Empresa 3");
    }

    [Theory]
    [InlineData(StatusTenant.Ativo, "Ativo")]
    [InlineData(StatusTenant.Inativo, "Inativo")]
    [InlineData(StatusTenant.Suspenso, "Suspenso")]
    public void DeveMappearTodosOsStatusCorretamente(StatusTenant status, string statusEsperado)
    {
        // Arrange
        var usuarioAlteracao = CriarIdUsuario();
        var organizacao = TestDataBuilders.CriarOrganizacao(
            status: status,
            usuarioUltimaAlteracao: usuarioAlteracao
        );

        // Act
        var dto = Mapper.Map<OrganizacaoDto>(organizacao);

        // Assert
        dto.Should().NotBeNull();
        dto.Status.Should().Be(statusEsperado);
    }
}