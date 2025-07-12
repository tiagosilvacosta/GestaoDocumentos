using FluentAssertions;
using Tsc.GestaoDocumentos.Application.Documentos;
using Tsc.GestaoDocumentos.Application.Tests.Mappings.Helpers;
using Tsc.GestaoDocumentos.Domain.Documentos;
using Xunit;

namespace Tsc.GestaoDocumentos.Application.Tests.Mappings;

/// <summary>
/// Testes para validar o mapeamento de DonoDocumento para DonoDocumentoDto
/// </summary>
public class DonoDocumentoMappingTests : MappingTestsBase
{
    [Fact]
    public void DeveMappearDonoDocumentoParaDonoDocumentoDto_ComSucesso()
    {
        // Arrange
        var idOrganizacao = CriarIdOrganizacao();
        var usuarioCriacao = CriarIdUsuario();
        var idTipoDono = CriarIdTipoDono();
        
        var tipoDono = TestDataBuilders.CriarTipoDono(
            id: idTipoDono,
            idOrganizacao: idOrganizacao,
            nome: "Pessoa Física",
            usuarioCriacao: usuarioCriacao
        );

        var donoDocumento = TestDataBuilders.CriarDonoDocumento(
            idOrganizacao: idOrganizacao,
            nomeAmigavel: "João Silva Santos",
            idTipoDono: idTipoDono,
            tipoDono: tipoDono,
            usuarioCriacao: usuarioCriacao
        );

        // Act
        var dto = Mapper.Map<DonoDocumentoDto>(donoDocumento);

        // Assert
        dto.Should().NotBeNull();
        dto.Id.Should().Be(donoDocumento.Id.Valor);
        dto.IdOrganizacao.Should().Be(donoDocumento.IdOrganizacao);
        dto.NomeAmigavel.Should().Be(donoDocumento.NomeAmigavel);
        dto.TipoDonoId.Should().Be(donoDocumento.IdTipoDono.Valor);
        dto.TipoDonoNome.Should().Be(tipoDono.Nome);
        dto.DataCriacao.Should().Be(donoDocumento.DataCriacao);
        dto.DataUltimaAlteracao.Should().Be(donoDocumento.DataAtualizacao);
        dto.UsuarioCriacao.Should().Be(donoDocumento.UsuarioCriacao.Valor);
        dto.UsuarioUltimaAlteracao.Should().Be(donoDocumento.UsuarioUltimaAlteracao.Valor);
        dto.DocumentosVinculados.Should().NotBeNull();
        dto.DocumentosVinculados.Should().BeEmpty(); // Sem vinculações inicialmente
    }

    [Fact]
    public void DeveMappearDonoDocumentoComDocumentosVinculados_ComSucesso()
    {
        // Arrange
        var idOrganizacao = CriarIdOrganizacao();
        var usuarioCriacao = CriarIdUsuario();
        var idTipoDono = CriarIdTipoDono();
        var idTipoDocumento = CriarIdTipoDocumento();
        
        var tipoDono = TestDataBuilders.CriarTipoDono(
            id: idTipoDono,
            idOrganizacao: idOrganizacao,
            usuarioCriacao: usuarioCriacao
        );

        var tipoDocumento = TestDataBuilders.CriarTipoDocumento(
            id: idTipoDocumento,
            idOrganizacao: idOrganizacao,
            nome: "CPF",
            usuarioCriacao: usuarioCriacao
        );
        tipoDocumento.VincularTipoDono(tipoDono);
        tipoDono.VincularTipoDocumento(tipoDocumento);

        var donoDocumento = TestDataBuilders.CriarDonoDocumento(
            idOrganizacao: idOrganizacao,
            nomeAmigavel: "Maria da Silva",
            idTipoDono: idTipoDono,
            tipoDono: tipoDono,
            usuarioCriacao: usuarioCriacao
        );

        var documento1 = TestDataBuilders.CriarDocumento(
            idOrganizacao: idOrganizacao,
            nomeArquivo: "cpf_maria.pdf",
            idTipoDocumento: idTipoDocumento,
            tipoDocumento: tipoDocumento,
            usuarioCriacao: usuarioCriacao
        );

        var documento2 = TestDataBuilders.CriarDocumento(
            idOrganizacao: idOrganizacao,
            nomeArquivo: "rg_maria.pdf",
            idTipoDocumento: idTipoDocumento,
            tipoDocumento: tipoDocumento,
            usuarioCriacao: usuarioCriacao
        );

        // Simular vinculação
        donoDocumento.VincularDocumento(documento1);
        donoDocumento.VincularDocumento(documento2);

        // Configurar as propriedades de navegação para o teste
        var vinculacoes = donoDocumento.DocumentosVinculados.ToList();
        foreach (var vinculacao in vinculacoes)
        {
            if (vinculacao.IdDocumento == documento1.Id)
                typeof(DocumentoDonoDocumento).GetProperty("Documento")?.SetValue(vinculacao, documento1);
            else if (vinculacao.IdDocumento == documento2.Id)
                typeof(DocumentoDonoDocumento).GetProperty("Documento")?.SetValue(vinculacao, documento2);
        }

        // Act
        var dto = Mapper.Map<DonoDocumentoDto>(donoDocumento);

        // Assert
        dto.Should().NotBeNull();
        dto.DocumentosVinculados.Should().HaveCount(2);
        dto.DocumentosVinculados.Should().Contain(d => d.NomeArquivo == "cpf_maria.pdf");
        dto.DocumentosVinculados.Should().Contain(d => d.NomeArquivo == "rg_maria.pdf");
    }

    [Fact]
    public void DeveMappearTipoDonoNomeCorretamente()
    {
        // Arrange
        var idOrganizacao = CriarIdOrganizacao();
        var usuarioCriacao = CriarIdUsuario();
        var idTipoDono = CriarIdTipoDono();
        
        var tipoDono = TestDataBuilders.CriarTipoDono(
            id: idTipoDono,
            idOrganizacao: idOrganizacao,
            nome: "Pessoa Jurídica",
            usuarioCriacao: usuarioCriacao
        );

        var donoDocumento = TestDataBuilders.CriarDonoDocumento(
            idOrganizacao: idOrganizacao,
            idTipoDono: idTipoDono,
            tipoDono: tipoDono,
            usuarioCriacao: usuarioCriacao
        );

        // Act
        var dto = Mapper.Map<DonoDocumentoDto>(donoDocumento);

        // Assert
        dto.Should().NotBeNull();
        dto.TipoDonoNome.Should().Be("Pessoa Jurídica");
    }

    [Fact]
    public void DeveMappearIdDonoDocumentoCorretamente()
    {
        // Arrange
        var idEspecifico = CriarIdDonoDocumento();
        var donoDocumento = TestDataBuilders.CriarDonoDocumento(id: idEspecifico);

        // Act
        var dto = Mapper.Map<DonoDocumentoDto>(donoDocumento);

        // Assert
        dto.Should().NotBeNull();
        dto.Id.Should().Be(idEspecifico.Valor);
    }

    [Fact]
    public void DeveMappearIdOrganizacaoCorretamente()
    {
        // Arrange
        var idOrganizacaoEspecifico = CriarIdOrganizacao();
        var donoDocumento = TestDataBuilders.CriarDonoDocumento(idOrganizacao: idOrganizacaoEspecifico);

        // Act
        var dto = Mapper.Map<DonoDocumentoDto>(donoDocumento);

        // Assert
        dto.Should().NotBeNull();
        dto.IdOrganizacao.Should().Be(idOrganizacaoEspecifico);
    }

    [Fact]
    public void DeveMappearTipoDonoIdCorretamente()
    {
        // Arrange
        var idTipoDonoEspecifico = CriarIdTipoDono();
        var donoDocumento = TestDataBuilders.CriarDonoDocumento(idTipoDono: idTipoDonoEspecifico);

        // Act
        var dto = Mapper.Map<DonoDocumentoDto>(donoDocumento);

        // Assert
        dto.Should().NotBeNull();
        dto.TipoDonoId.Should().Be(idTipoDonoEspecifico.Valor);
    }

    [Fact]
    public void DeveMappearNomeAmigavelCorretamente()
    {
        // Arrange
        var nomeEspecifico = "Carlos Eduardo da Costa";
        var donoDocumento = TestDataBuilders.CriarDonoDocumento(nomeAmigavel: nomeEspecifico);

        // Act
        var dto = Mapper.Map<DonoDocumentoDto>(donoDocumento);

        // Assert
        dto.Should().NotBeNull();
        dto.NomeAmigavel.Should().Be(nomeEspecifico);
    }

    [Fact]
    public void DeveMappearCamposDeAuditoriaCorretamente()
    {
        // Arrange
        var usuarioCriacao = CriarIdUsuario();
        var donoDocumento = TestDataBuilders.CriarDonoDocumento(usuarioCriacao: usuarioCriacao);

        // Act
        var dto = Mapper.Map<DonoDocumentoDto>(donoDocumento);

        // Assert
        dto.Should().NotBeNull();
        dto.UsuarioCriacao.Should().Be(usuarioCriacao.Valor);
        dto.UsuarioUltimaAlteracao.Should().Be(usuarioCriacao.Valor);
        dto.DataCriacao.Should().Be(donoDocumento.DataCriacao);
        dto.DataUltimaAlteracao.Should().Be(donoDocumento.DataAtualizacao);
    }

    [Fact]
    public void DeveMappearListaDeDonosDocumento_ComSucesso()
    {
        // Arrange
        var idOrganizacao = CriarIdOrganizacao();
        var usuarioCriacao = CriarIdUsuario();
        var idTipoDono = CriarIdTipoDono();
        
        var tipoDono = TestDataBuilders.CriarTipoDono(
            id: idTipoDono,
            idOrganizacao: idOrganizacao,
            nome: "Pessoa Física",
            usuarioCriacao: usuarioCriacao
        );

        var donosDocumento = new List<DonoDocumento>
        {
            TestDataBuilders.CriarDonoDocumento(
                idOrganizacao: idOrganizacao,
                nomeAmigavel: "João Silva",
                idTipoDono: idTipoDono,
                tipoDono: tipoDono,
                usuarioCriacao: usuarioCriacao
            ),
            TestDataBuilders.CriarDonoDocumento(
                idOrganizacao: idOrganizacao,
                nomeAmigavel: "Maria Santos",
                idTipoDono: idTipoDono,
                tipoDono: tipoDono,
                usuarioCriacao: usuarioCriacao
            ),
            TestDataBuilders.CriarDonoDocumento(
                idOrganizacao: idOrganizacao,
                nomeAmigavel: "Pedro Costa",
                idTipoDono: idTipoDono,
                tipoDono: tipoDono,
                usuarioCriacao: usuarioCriacao
            )
        };

        // Act
        var dtos = Mapper.Map<List<DonoDocumentoDto>>(donosDocumento);

        // Assert
        dtos.Should().NotBeNull();
        dtos.Should().HaveCount(3);
        dtos[0].NomeAmigavel.Should().Be("João Silva");
        dtos[1].NomeAmigavel.Should().Be("Maria Santos");
        dtos[2].NomeAmigavel.Should().Be("Pedro Costa");
        dtos.Should().AllSatisfy(d => d.TipoDonoNome.Should().Be("Pessoa Física"));
    }

    [Fact]
    public void DeveMappearDonoDocumentoSemDocumentosVinculados_ComListaVazia()
    {
        // Arrange
        var donoDocumento = TestDataBuilders.CriarDonoDocumento(nomeAmigavel: "Dono sem documentos");

        // Act
        var dto = Mapper.Map<DonoDocumentoDto>(donoDocumento);

        // Assert
        dto.Should().NotBeNull();
        dto.DocumentosVinculados.Should().NotBeNull();
        dto.DocumentosVinculados.Should().BeEmpty();
    }

    [Fact]
    public void DeveMappearPropriedadesDeNavegacaoCorretamente()
    {
        // Arrange
        var idOrganizacao = CriarIdOrganizacao();
        var usuarioCriacao = CriarIdUsuario();
        var idTipoDono = CriarIdTipoDono();
        var idTipoDocumento = CriarIdTipoDocumento();
        
        var tipoDono = TestDataBuilders.CriarTipoDono(
            id: idTipoDono,
            idOrganizacao: idOrganizacao,
            nome: "Entidade Governamental",
            usuarioCriacao: usuarioCriacao
        );

        var tipoDocumento = TestDataBuilders.CriarTipoDocumento(
            id: idTipoDocumento,
            idOrganizacao: idOrganizacao,
            nome: "Alvará",
            usuarioCriacao: usuarioCriacao
        );

        tipoDocumento.VincularTipoDono(tipoDono);
        tipoDono.VincularTipoDocumento(tipoDocumento);

        var donoDocumento = TestDataBuilders.CriarDonoDocumento(
            idOrganizacao: idOrganizacao,
            nomeAmigavel: "Prefeitura Municipal",
            idTipoDono: idTipoDono,
            tipoDono: tipoDono,
            usuarioCriacao: usuarioCriacao
        );

        var documento = TestDataBuilders.CriarDocumento(
            idOrganizacao: idOrganizacao,
            nomeArquivo: "alvara_funcionamento.pdf",
            idTipoDocumento: idTipoDocumento,
            tipoDocumento: tipoDocumento,
            usuarioCriacao: usuarioCriacao
        );

        donoDocumento.VincularDocumento(documento);

        // Configurar propriedade de navegação
        var vinculacao = donoDocumento.DocumentosVinculados.First();
        typeof(DocumentoDonoDocumento).GetProperty("Documento")?.SetValue(vinculacao, documento);

        // Act
        var dto = Mapper.Map<DonoDocumentoDto>(donoDocumento);

        // Assert
        dto.Should().NotBeNull();
        dto.TipoDonoNome.Should().Be("Entidade Governamental");
        dto.DocumentosVinculados.Should().HaveCount(1);
        var documentoVinculado = dto.DocumentosVinculados.First();
        documentoVinculado.Id.Should().Be(documento.Id.Valor);
        documentoVinculado.NomeArquivo.Should().Be("alvara_funcionamento.pdf");
    }

    [Fact]
    public void NaoDeveGerarReferenciaCircular_QuandoMapearDocumentosVinculados()
    {
        // Arrange
        var idOrganizacao = CriarIdOrganizacao();
        var usuarioCriacao = CriarIdUsuario();
        var idTipoDono = CriarIdTipoDono();
        var idTipoDocumento = CriarIdTipoDocumento();
        
        var tipoDono = TestDataBuilders.CriarTipoDono(
            id: idTipoDono,
            idOrganizacao: idOrganizacao,
            usuarioCriacao: usuarioCriacao
        );

        var tipoDocumento = TestDataBuilders.CriarTipoDocumento(
            id: idTipoDocumento,
            idOrganizacao: idOrganizacao,
            usuarioCriacao: usuarioCriacao
        );
        tipoDocumento.VincularTipoDono(tipoDono);
        tipoDono.VincularTipoDocumento(tipoDocumento);

        var donoDocumento = TestDataBuilders.CriarDonoDocumento(
            idOrganizacao: idOrganizacao,
            idTipoDono: idTipoDono,
            tipoDono: tipoDono,
            usuarioCriacao: usuarioCriacao
        );

        var documento = TestDataBuilders.CriarDocumento(
            idOrganizacao: idOrganizacao,
            idTipoDocumento: idTipoDocumento,
            tipoDocumento: tipoDocumento,
            usuarioCriacao: usuarioCriacao
        );

        // Criar vinculação bidirecional
        donoDocumento.VincularDocumento(documento);
        documento.VincularDonoDocumento(donoDocumento);

        // Configurar propriedades de navegação
        var vinculacaoDonoDocumento = donoDocumento.DocumentosVinculados.First();
        typeof(DocumentoDonoDocumento).GetProperty("Documento")?.SetValue(vinculacaoDonoDocumento, documento);

        var vinculacaoDocumento = documento.DonosVinculados.First();
        typeof(DocumentoDonoDocumento).GetProperty("DonoDocumento")?.SetValue(vinculacaoDocumento, donoDocumento);

        // Act & Assert - Não deve gerar exceção de referência circular
        Action act = () => Mapper.Map<DonoDocumentoDto>(donoDocumento);
        act.Should().NotThrow();

        var dto = Mapper.Map<DonoDocumentoDto>(donoDocumento);
        dto.Should().NotBeNull();
        dto.DocumentosVinculados.Should().HaveCount(1);
    }
}