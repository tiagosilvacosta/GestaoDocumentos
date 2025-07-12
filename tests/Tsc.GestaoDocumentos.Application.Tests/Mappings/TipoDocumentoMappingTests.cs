using FluentAssertions;
using Tsc.GestaoDocumentos.Application.Documentos;
using Tsc.GestaoDocumentos.Application.Tests.Mappings.Helpers;
using Tsc.GestaoDocumentos.Domain.Documentos;
using Xunit;

namespace Tsc.GestaoDocumentos.Application.Tests.Mappings;

/// <summary>
/// Testes para validar o mapeamento de TipoDocumento para TipoDocumentoDto
/// </summary>
public class TipoDocumentoMappingTests : MappingTestsBase
{
    [Fact]
    public void DeveMappearTipoDocumentoParaTipoDocumentoDto_ComSucesso()
    {
        // Arrange
        var idOrganizacao = CriarIdOrganizacao();
        var usuarioCriacao = CriarIdUsuario();
        
        var tipoDocumento = TestDataBuilders.CriarTipoDocumento(
            idOrganizacao: idOrganizacao,
            nome: "CPF",
            permiteMultiplosDocumentos: false,
            usuarioCriacao: usuarioCriacao
        );

        // Act
        var dto = Mapper.Map<TipoDocumentoDto>(tipoDocumento);

        // Assert
        dto.Should().NotBeNull();
        dto.Id.Should().Be(tipoDocumento.Id.Valor);
        dto.IdOrganizacao.Should().Be(tipoDocumento.IdOrganizacao);
        dto.Nome.Should().Be(tipoDocumento.Nome);
        dto.PermiteMultiplosDocumentos.Should().Be(tipoDocumento.PermiteMultiplosDocumentos);
        dto.DataCriacao.Should().Be(tipoDocumento.DataCriacao);
        dto.DataUltimaAlteracao.Should().Be(tipoDocumento.DataAtualizacao);
        dto.UsuarioCriacao.Should().Be(tipoDocumento.UsuarioCriacao.Valor);
        dto.UsuarioUltimaAlteracao.Should().Be(tipoDocumento.UsuarioUltimaAlteracao.Valor);
        dto.TiposDonoVinculados.Should().NotBeNull();
        dto.TiposDonoVinculados.Should().BeEmpty(); // Sem vinculações inicialmente
    }

    [Fact]
    public void DeveMappearTipoDocumentoComTiposDonoVinculados_ComSucesso()
    {
        // Arrange
        var idOrganizacao = CriarIdOrganizacao();
        var usuarioCriacao = CriarIdUsuario();
        
        var tipoDocumento = TestDataBuilders.CriarTipoDocumento(
            idOrganizacao: idOrganizacao,
            nome: "Nota Fiscal",
            permiteMultiplosDocumentos: true,
            usuarioCriacao: usuarioCriacao
        );

        var tipoDono1 = TestDataBuilders.CriarTipoDono(
            idOrganizacao: idOrganizacao,
            nome: "Pessoa Física",
            usuarioCriacao: usuarioCriacao
        );

        var tipoDono2 = TestDataBuilders.CriarTipoDono(
            idOrganizacao: idOrganizacao,
            nome: "Pessoa Jurídica",
            usuarioCriacao: usuarioCriacao
        );

        // Simular vinculação
        tipoDocumento.VincularTipoDono(tipoDono1);
        tipoDocumento.VincularTipoDono(tipoDono2);

        // Configurar as propriedades de navegação para o teste
        var vinculacoes = tipoDocumento.TiposDonoVinculados.ToList();
        foreach (var vinculacao in vinculacoes)
        {
            if (vinculacao.IdTipoDono == tipoDono1.Id)
                typeof(TipoDonoTipoDocumento).GetProperty("TipoDono")?.SetValue(vinculacao, tipoDono1);
            else if (vinculacao.IdTipoDono == tipoDono2.Id)
                typeof(TipoDonoTipoDocumento).GetProperty("TipoDono")?.SetValue(vinculacao, tipoDono2);
        }

        // Act
        var dto = Mapper.Map<TipoDocumentoDto>(tipoDocumento);

        // Assert
        dto.Should().NotBeNull();
        dto.TiposDonoVinculados.Should().HaveCount(2);
        dto.TiposDonoVinculados.Should().Contain(td => td.Nome == "Pessoa Física");
        dto.TiposDonoVinculados.Should().Contain(td => td.Nome == "Pessoa Jurídica");
    }

    [Fact]
    public void DeveMappearTipoDocumentoQuePermiteMultiplosDocumentos_ComSucesso()
    {
        // Arrange
        var tipoDocumento = TestDataBuilders.CriarTipoDocumento(
            nome: "Foto",
            permiteMultiplosDocumentos: true
        );

        // Act
        var dto = Mapper.Map<TipoDocumentoDto>(tipoDocumento);

        // Assert
        dto.Should().NotBeNull();
        dto.PermiteMultiplosDocumentos.Should().BeTrue();
    }

    [Fact]
    public void DeveMappearTipoDocumentoQueNaoPermiteMultiplosDocumentos_ComSucesso()
    {
        // Arrange
        var tipoDocumento = TestDataBuilders.CriarTipoDocumento(
            nome: "RG",
            permiteMultiplosDocumentos: false
        );

        // Act
        var dto = Mapper.Map<TipoDocumentoDto>(tipoDocumento);

        // Assert
        dto.Should().NotBeNull();
        dto.PermiteMultiplosDocumentos.Should().BeFalse();
    }

    [Fact]
    public void DeveMappearIdTipoDocumentoCorretamente()
    {
        // Arrange
        var idEspecifico = CriarIdTipoDocumento();
        var tipoDocumento = TestDataBuilders.CriarTipoDocumento(id: idEspecifico);

        // Act
        var dto = Mapper.Map<TipoDocumentoDto>(tipoDocumento);

        // Assert
        dto.Should().NotBeNull();
        dto.Id.Should().Be(idEspecifico.Valor);
    }

    [Fact]
    public void DeveMappearIdOrganizacaoCorretamente()
    {
        // Arrange
        var idOrganizacaoEspecifico = CriarIdOrganizacao();
        var tipoDocumento = TestDataBuilders.CriarTipoDocumento(idOrganizacao: idOrganizacaoEspecifico);

        // Act
        var dto = Mapper.Map<TipoDocumentoDto>(tipoDocumento);

        // Assert
        dto.Should().NotBeNull();
        dto.IdOrganizacao.Should().Be(idOrganizacaoEspecifico);
    }

    [Fact]
    public void DeveMappearNomeCorretamente()
    {
        // Arrange
        var nomeEspecifico = "Carteira de Trabalho";
        var tipoDocumento = TestDataBuilders.CriarTipoDocumento(nome: nomeEspecifico);

        // Act
        var dto = Mapper.Map<TipoDocumentoDto>(tipoDocumento);

        // Assert
        dto.Should().NotBeNull();
        dto.Nome.Should().Be(nomeEspecifico);
    }

    [Fact]
    public void DeveMappearCamposDeAuditoriaCorretamente()
    {
        // Arrange
        var usuarioCriacao = CriarIdUsuario();
        var tipoDocumento = TestDataBuilders.CriarTipoDocumento(usuarioCriacao: usuarioCriacao);

        // Act
        var dto = Mapper.Map<TipoDocumentoDto>(tipoDocumento);

        // Assert
        dto.Should().NotBeNull();
        dto.UsuarioCriacao.Should().Be(usuarioCriacao.Valor);
        dto.UsuarioUltimaAlteracao.Should().Be(usuarioCriacao.Valor);
        dto.DataCriacao.Should().Be(tipoDocumento.DataCriacao);
        dto.DataUltimaAlteracao.Should().Be(tipoDocumento.DataAtualizacao);
    }

    [Fact]
    public void DeveMappearListaDeTiposDocumento_ComSucesso()
    {
        // Arrange
        var tiposDocumento = new List<TipoDocumento>
        {
            TestDataBuilders.CriarTipoDocumento(nome: "CPF", permiteMultiplosDocumentos: false),
            TestDataBuilders.CriarTipoDocumento(nome: "RG", permiteMultiplosDocumentos: false),
            TestDataBuilders.CriarTipoDocumento(nome: "Foto", permiteMultiplosDocumentos: true)
        };

        // Act
        var dtos = Mapper.Map<List<TipoDocumentoDto>>(tiposDocumento);

        // Assert
        dtos.Should().NotBeNull();
        dtos.Should().HaveCount(3);
        dtos[0].Nome.Should().Be("CPF");
        dtos[0].PermiteMultiplosDocumentos.Should().BeFalse();
        dtos[1].Nome.Should().Be("RG");
        dtos[1].PermiteMultiplosDocumentos.Should().BeFalse();
        dtos[2].Nome.Should().Be("Foto");
        dtos[2].PermiteMultiplosDocumentos.Should().BeTrue();
    }

    [Fact]
    public void DeveMappearTipoDocumentoSemVinculacoes_ComListaVazia()
    {
        // Arrange
        var tipoDocumento = TestDataBuilders.CriarTipoDocumento(nome: "Tipo sem vinculações");

        // Act
        var dto = Mapper.Map<TipoDocumentoDto>(tipoDocumento);

        // Assert
        dto.Should().NotBeNull();
        dto.TiposDonoVinculados.Should().NotBeNull();
        dto.TiposDonoVinculados.Should().BeEmpty();
    }

    [Fact]
    public void DeveMappearPropriedadesDeNavegacaoCorretamente()
    {
        // Arrange
        var idOrganizacao = CriarIdOrganizacao();
        var usuarioCriacao = CriarIdUsuario();
        
        var tipoDocumento = TestDataBuilders.CriarTipoDocumento(
            idOrganizacao: idOrganizacao,
            nome: "Passaporte",
            usuarioCriacao: usuarioCriacao
        );

        var tipoDono = TestDataBuilders.CriarTipoDono(
            idOrganizacao: idOrganizacao,
            nome: "Pessoa Física",
            usuarioCriacao: usuarioCriacao
        );

        tipoDocumento.VincularTipoDono(tipoDono);

        // Configurar propriedade de navegação
        var vinculacao = tipoDocumento.TiposDonoVinculados.First();
        typeof(TipoDonoTipoDocumento).GetProperty("TipoDono")?.SetValue(vinculacao, tipoDono);

        // Act
        var dto = Mapper.Map<TipoDocumentoDto>(tipoDocumento);

        // Assert
        dto.Should().NotBeNull();
        dto.TiposDonoVinculados.Should().HaveCount(1);
        var tipoDonoVinculado = dto.TiposDonoVinculados.First();
        tipoDonoVinculado.Id.Should().Be(tipoDono.Id.Valor);
        tipoDonoVinculado.Nome.Should().Be("Pessoa Física");
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void DeveMappearPermiteMultiplosDocumentosCorretamente(bool permiteMultiplos)
    {
        // Arrange
        var tipoDocumento = TestDataBuilders.CriarTipoDocumento(
            permiteMultiplosDocumentos: permiteMultiplos
        );

        // Act
        var dto = Mapper.Map<TipoDocumentoDto>(tipoDocumento);

        // Assert
        dto.Should().NotBeNull();
        dto.PermiteMultiplosDocumentos.Should().Be(permiteMultiplos);
    }

    [Fact]
    public void NaoDeveGerarReferenciaCircular_QuandoMapearVinculacoes()
    {
        // Arrange
        var idOrganizacao = CriarIdOrganizacao();
        var usuarioCriacao = CriarIdUsuario();
        
        var tipoDocumento = TestDataBuilders.CriarTipoDocumento(
            idOrganizacao: idOrganizacao,
            usuarioCriacao: usuarioCriacao
        );

        var tipoDono = TestDataBuilders.CriarTipoDono(
            idOrganizacao: idOrganizacao,
            usuarioCriacao: usuarioCriacao
        );

        // Criar vinculação bidirecional
        tipoDocumento.VincularTipoDono(tipoDono);
        tipoDono.VincularTipoDocumento(tipoDocumento);

        // Configurar propriedades de navegação
        var vinculacaoTipoDocumento = tipoDocumento.TiposDonoVinculados.First();
        typeof(TipoDonoTipoDocumento).GetProperty("TipoDono")?.SetValue(vinculacaoTipoDocumento, tipoDono);

        var vinculacaoTipoDono = tipoDono.TiposDocumentoVinculados.First();
        typeof(TipoDonoTipoDocumento).GetProperty("TipoDocumento")?.SetValue(vinculacaoTipoDono, tipoDocumento);

        // Act & Assert - Não deve gerar exceção de referência circular
        Action act = () => Mapper.Map<TipoDocumentoDto>(tipoDocumento);
        act.Should().NotThrow();

        var dto = Mapper.Map<TipoDocumentoDto>(tipoDocumento);
        dto.Should().NotBeNull();
        dto.TiposDonoVinculados.Should().HaveCount(1);
    }
}