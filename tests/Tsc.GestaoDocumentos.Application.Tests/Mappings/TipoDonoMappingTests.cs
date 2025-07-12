using FluentAssertions;
using Tsc.GestaoDocumentos.Application.Documentos;
using Tsc.GestaoDocumentos.Application.Tests.Mappings.Helpers;
using Tsc.GestaoDocumentos.Domain.Documentos;
using Xunit;

namespace Tsc.GestaoDocumentos.Application.Tests.Mappings;

/// <summary>
/// Testes para validar o mapeamento de TipoDono para TipoDonoDto
/// </summary>
public class TipoDonoMappingTests : MappingTestsBase
{
    [Fact]
    public void DeveMappearTipoDonoParaTipoDonoDto_ComSucesso()
    {
        // Arrange
        var idOrganizacao = CriarIdOrganizacao();
        var usuarioCriacao = CriarIdUsuario();
        
        var tipoDono = TestDataBuilders.CriarTipoDono(
            idOrganizacao: idOrganizacao,
            nome: "Pessoa Física",
            usuarioCriacao: usuarioCriacao
        );

        // Act
        var dto = Mapper.Map<TipoDonoDto>(tipoDono);

        // Assert
        dto.Should().NotBeNull();
        dto.Id.Should().Be(tipoDono.Id.Valor);
        dto.IdOrganizacao.Should().Be(tipoDono.IdOrganizacao);
        dto.Nome.Should().Be(tipoDono.Nome);
        dto.DataCriacao.Should().Be(tipoDono.DataCriacao);
        dto.DataUltimaAlteracao.Should().Be(tipoDono.DataAtualizacao);
        dto.UsuarioCriacao.Should().Be(tipoDono.UsuarioCriacao.Valor);
        dto.UsuarioUltimaAlteracao.Should().Be(tipoDono.UsuarioUltimaAlteracao.Valor);
        dto.TiposDocumentoVinculados.Should().NotBeNull();
        dto.TiposDocumentoVinculados.Should().BeEmpty(); // Sem vinculações inicialmente
    }

    [Fact]
    public void DeveMappearTipoDonoComTiposDocumentoVinculados_ComSucesso()
    {
        // Arrange
        var idOrganizacao = CriarIdOrganizacao();
        var usuarioCriacao = CriarIdUsuario();
        
        var tipoDono = TestDataBuilders.CriarTipoDono(
            idOrganizacao: idOrganizacao,
            nome: "Pessoa Jurídica",
            usuarioCriacao: usuarioCriacao
        );

        var tipoDocumento1 = TestDataBuilders.CriarTipoDocumento(
            idOrganizacao: idOrganizacao,
            nome: "CNPJ",
            usuarioCriacao: usuarioCriacao
        );

        var tipoDocumento2 = TestDataBuilders.CriarTipoDocumento(
            idOrganizacao: idOrganizacao,
            nome: "Contrato Social",
            usuarioCriacao: usuarioCriacao
        );

        // Simular vinculação - em um cenário real isso seria feito através dos métodos de domínio
        tipoDono.VincularTipoDocumento(tipoDocumento1);
        tipoDono.VincularTipoDocumento(tipoDocumento2);

        // Configurar as propriedades de navegação para o teste
        var vinculacoes = tipoDono.TiposDocumentoVinculados.ToList();
        foreach (var vinculacao in vinculacoes)
        {
            if (vinculacao.IdTipoDocumento == tipoDocumento1.Id)
                typeof(TipoDonoTipoDocumento).GetProperty("TipoDocumento")?.SetValue(vinculacao, tipoDocumento1);
            else if (vinculacao.IdTipoDocumento == tipoDocumento2.Id)
                typeof(TipoDonoTipoDocumento).GetProperty("TipoDocumento")?.SetValue(vinculacao, tipoDocumento2);
        }

        // Act
        var dto = Mapper.Map<TipoDonoDto>(tipoDono);

        // Assert
        dto.Should().NotBeNull();
        dto.TiposDocumentoVinculados.Should().HaveCount(2);
        dto.TiposDocumentoVinculados.Should().Contain(td => td.Nome == "CNPJ");
        dto.TiposDocumentoVinculados.Should().Contain(td => td.Nome == "Contrato Social");
    }

    [Fact]
    public void DeveMappearIdTipoDonoCorretamente()
    {
        // Arrange
        var idEspecifico = CriarIdTipoDono();
        var tipoDono = TestDataBuilders.CriarTipoDono(id: idEspecifico);

        // Act
        var dto = Mapper.Map<TipoDonoDto>(tipoDono);

        // Assert
        dto.Should().NotBeNull();
        dto.Id.Should().Be(idEspecifico.Valor);
    }

    [Fact]
    public void DeveMappearIdOrganizacaoCorretamente()
    {
        // Arrange
        var idOrganizacaoEspecifico = CriarIdOrganizacao();
        var tipoDono = TestDataBuilders.CriarTipoDono(idOrganizacao: idOrganizacaoEspecifico);

        // Act
        var dto = Mapper.Map<TipoDonoDto>(tipoDono);

        // Assert
        dto.Should().NotBeNull();
        dto.IdOrganizacao.Should().Be(idOrganizacaoEspecifico);
    }

    [Fact]
    public void DeveMappearNomeCorretamente()
    {
        // Arrange
        var nomeEspecifico = "Entidade Governamental";
        var tipoDono = TestDataBuilders.CriarTipoDono(nome: nomeEspecifico);

        // Act
        var dto = Mapper.Map<TipoDonoDto>(tipoDono);

        // Assert
        dto.Should().NotBeNull();
        dto.Nome.Should().Be(nomeEspecifico);
    }

    [Fact]
    public void DeveMappearCamposDeAuditoriaCorretamente()
    {
        // Arrange
        var usuarioCriacao = CriarIdUsuario();
        var tipoDono = TestDataBuilders.CriarTipoDono(usuarioCriacao: usuarioCriacao);

        // Act
        var dto = Mapper.Map<TipoDonoDto>(tipoDono);

        // Assert
        dto.Should().NotBeNull();
        dto.UsuarioCriacao.Should().Be(usuarioCriacao.Valor);
        dto.UsuarioUltimaAlteracao.Should().Be(usuarioCriacao.Valor);
        dto.DataCriacao.Should().Be(tipoDono.DataCriacao);
        dto.DataUltimaAlteracao.Should().Be(tipoDono.DataAtualizacao);
    }

    [Fact]
    public void DeveMappearListaDeTiposDono_ComSucesso()
    {
        // Arrange
        var tiposDono = new List<TipoDono>
        {
            TestDataBuilders.CriarTipoDono(nome: "Pessoa Física"),
            TestDataBuilders.CriarTipoDono(nome: "Pessoa Jurídica"),
            TestDataBuilders.CriarTipoDono(nome: "Órgão Público")
        };

        // Act
        var dtos = Mapper.Map<List<TipoDonoDto>>(tiposDono);

        // Assert
        dtos.Should().NotBeNull();
        dtos.Should().HaveCount(3);
        dtos[0].Nome.Should().Be("Pessoa Física");
        dtos[1].Nome.Should().Be("Pessoa Jurídica");
        dtos[2].Nome.Should().Be("Órgão Público");
    }

    [Fact]
    public void DeveMappearTipoDonoSemVinculacoes_ComListaVazia()
    {
        // Arrange
        var tipoDono = TestDataBuilders.CriarTipoDono(nome: "Tipo sem vinculações");

        // Act
        var dto = Mapper.Map<TipoDonoDto>(tipoDono);

        // Assert
        dto.Should().NotBeNull();
        dto.TiposDocumentoVinculados.Should().NotBeNull();
        dto.TiposDocumentoVinculados.Should().BeEmpty();
    }

    [Fact]
    public void DeveMappearPropriedadesDeNavegacaoCorretamente()
    {
        // Arrange
        var idOrganizacao = CriarIdOrganizacao();
        var usuarioCriacao = CriarIdUsuario();
        
        var tipoDono = TestDataBuilders.CriarTipoDono(
            idOrganizacao: idOrganizacao,
            usuarioCriacao: usuarioCriacao
        );

        var tipoDocumento = TestDataBuilders.CriarTipoDocumento(
            idOrganizacao: idOrganizacao,
            nome: "Documento Teste",
            permiteMultiplosDocumentos: true,
            usuarioCriacao: usuarioCriacao
        );

        tipoDono.VincularTipoDocumento(tipoDocumento);

        // Configurar propriedade de navegação
        var vinculacao = tipoDono.TiposDocumentoVinculados.First();
        typeof(TipoDonoTipoDocumento).GetProperty("TipoDocumento")?.SetValue(vinculacao, tipoDocumento);

        // Act
        var dto = Mapper.Map<TipoDonoDto>(tipoDono);

        // Assert
        dto.Should().NotBeNull();
        dto.TiposDocumentoVinculados.Should().HaveCount(1);
        var tipoDocumentoVinculado = dto.TiposDocumentoVinculados.First();
        tipoDocumentoVinculado.Id.Should().Be(tipoDocumento.Id.Valor);
        tipoDocumentoVinculado.Nome.Should().Be("Documento Teste");
        tipoDocumentoVinculado.PermiteMultiplosDocumentos.Should().BeTrue();
    }

    [Fact]
    public void NaoDeveGerarReferenciaCircular_QuandoMapearVinculacoes()
    {
        // Arrange
        var idOrganizacao = CriarIdOrganizacao();
        var usuarioCriacao = CriarIdUsuario();
        
        var tipoDono = TestDataBuilders.CriarTipoDono(
            idOrganizacao: idOrganizacao,
            usuarioCriacao: usuarioCriacao
        );

        var tipoDocumento = TestDataBuilders.CriarTipoDocumento(
            idOrganizacao: idOrganizacao,
            usuarioCriacao: usuarioCriacao
        );

        // Criar vinculação bidirecional
        tipoDono.VincularTipoDocumento(tipoDocumento);
        tipoDocumento.VincularTipoDono(tipoDono);

        // Configurar propriedades de navegação
        var vinculacaoTipoDono = tipoDono.TiposDocumentoVinculados.First();
        typeof(TipoDonoTipoDocumento).GetProperty("TipoDocumento")?.SetValue(vinculacaoTipoDono, tipoDocumento);

        var vinculacaoTipoDocumento = tipoDocumento.TiposDonoVinculados.First();
        typeof(TipoDonoTipoDocumento).GetProperty("TipoDono")?.SetValue(vinculacaoTipoDocumento, tipoDono);

        // Act & Assert - Não deve gerar exceção de referência circular
        Action act = () => Mapper.Map<TipoDonoDto>(tipoDono);
        act.Should().NotThrow();

        var dto = Mapper.Map<TipoDonoDto>(tipoDono);
        dto.Should().NotBeNull();
        dto.TiposDocumentoVinculados.Should().HaveCount(1);
    }
}