using FluentAssertions;
using Tsc.GestaoDocumentos.Application.Documentos;
using Tsc.GestaoDocumentos.Application.Tests.Mappings.Helpers;
using Tsc.GestaoDocumentos.Domain.Documentos;
using Xunit;

namespace Tsc.GestaoDocumentos.Application.Tests.Mappings;

/// <summary>
/// Testes para validar o mapeamento de Documento para DocumentoDto
/// </summary>
public class DocumentoMappingTests : MappingTestsBase
{
    [Fact]
    public void DeveMappearDocumentoParaDocumentoDto_ComSucesso()
    {
        // Arrange
        var idOrganizacao = CriarIdOrganizacao();
        var usuarioCriacao = CriarIdUsuario();
        var idTipoDocumento = CriarIdTipoDocumento();
        var dataUpload = DateTime.UtcNow.AddHours(-1);
        var tamanhoArquivo = 1024000L; // 1MB
        
        var tipoDocumento = TestDataBuilders.CriarTipoDocumento(
            id: idTipoDocumento,
            idOrganizacao: idOrganizacao,
            nome: "Nota Fiscal",
            usuarioCriacao: usuarioCriacao
        );

        var documento = TestDataBuilders.CriarDocumento(
            idOrganizacao: idOrganizacao,
            nomeArquivo: "nota_fiscal_001.pdf",
            chaveArmazenamento: "/uploads/2024/01/nota_fiscal_001.pdf",
            dataUpload: dataUpload,
            tamanhoArquivo: tamanhoArquivo,
            tipoArquivo: "pdf",
            versao: 2,
            status: StatusDocumento.Ativo,
            idTipoDocumento: idTipoDocumento,
            tipoDocumento: tipoDocumento,
            usuarioCriacao: usuarioCriacao
        );

        // Act
        var dto = Mapper.Map<DocumentoDto>(documento);

        // Assert
        dto.Should().NotBeNull();
        dto.Id.Should().Be(documento.Id.Valor);
        dto.IdOrganizacao.Should().Be(documento.IdOrganizacao);
        dto.NomeArquivo.Should().Be(documento.NomeArquivo);
        dto.ChaveArmazenamento.Should().Be(documento.ChaveArmazenamento);
        dto.DataUpload.Should().Be(documento.DataUpload);
        dto.TamanhoArquivo.Should().Be(documento.TamanhoArquivo);
        dto.TipoArquivo.Should().Be(documento.TipoArquivo);
        dto.Versao.Should().Be(documento.Versao);
        dto.Status.Should().Be(documento.Status.ToString());
        dto.IdTipoDocumento.Should().Be(documento.IdTipoDocumento);
        dto.TipoDocumentoNome.Should().Be(tipoDocumento.Nome);
        dto.TamanhoFormatado.Should().Be(documento.ObterTamanhoFormatado());
        dto.DataCriacao.Should().Be(documento.DataCriacao);
        dto.DataUltimaAlteracao.Should().Be(documento.DataAtualizacao);
        dto.UsuarioCriacao.Should().Be(documento.UsuarioCriacao.Valor);
        dto.UsuarioUltimaAlteracao.Should().Be(documento.UsuarioUltimaAlteracao.Valor);
        dto.DonosVinculados.Should().NotBeNull();
        dto.DonosVinculados.Should().BeEmpty(); // Sem vinculações inicialmente
    }

    [Fact]
    public void DeveMappearDocumentoComDonosVinculados_ComSucesso()
    {
        // Arrange
        var idOrganizacao = CriarIdOrganizacao();
        var usuarioCriacao = CriarIdUsuario();
        var idTipoDocumento = CriarIdTipoDocumento();
        var idTipoDono = CriarIdTipoDono();
        
        var tipoDocumento = TestDataBuilders.CriarTipoDocumento(
            id: idTipoDocumento,
            idOrganizacao: idOrganizacao,
            nome: "RG",
            usuarioCriacao: usuarioCriacao
        );

        var tipoDono = TestDataBuilders.CriarTipoDono(
            id: idTipoDono,
            idOrganizacao: idOrganizacao,
            nome: "Pessoa Física",
            usuarioCriacao: usuarioCriacao
        );

        var documento = TestDataBuilders.CriarDocumento(
            idOrganizacao: idOrganizacao,
            nomeArquivo: "rg_frente.jpg",
            idTipoDocumento: idTipoDocumento,
            tipoDocumento: tipoDocumento,
            usuarioCriacao: usuarioCriacao
        );

        var donoDocumento1 = TestDataBuilders.CriarDonoDocumento(
            idOrganizacao: idOrganizacao,
            nomeAmigavel: "João Silva",
            idTipoDono: idTipoDono,
            tipoDono: tipoDono,
            usuarioCriacao: usuarioCriacao
        );

        var donoDocumento2 = TestDataBuilders.CriarDonoDocumento(
            idOrganizacao: idOrganizacao,
            nomeAmigavel: "Maria Silva",
            idTipoDono: idTipoDono,
            tipoDono: tipoDono,
            usuarioCriacao: usuarioCriacao
        );

        // Simular vinculação
        documento.VincularDonoDocumento(donoDocumento1);
        documento.VincularDonoDocumento(donoDocumento2);

        // Configurar as propriedades de navegação para o teste
        var vinculacoes = documento.DonosVinculados.ToList();
        foreach (var vinculacao in vinculacoes)
        {
            if (vinculacao.IdDonoDocumento == donoDocumento1.Id)
                typeof(DocumentoDonoDocumento).GetProperty("DonoDocumento")?.SetValue(vinculacao, donoDocumento1);
            else if (vinculacao.IdDonoDocumento == donoDocumento2.Id)
                typeof(DocumentoDonoDocumento).GetProperty("DonoDocumento")?.SetValue(vinculacao, donoDocumento2);
        }

        // Act
        var dto = Mapper.Map<DocumentoDto>(documento);

        // Assert
        dto.Should().NotBeNull();
        dto.DonosVinculados.Should().HaveCount(2);
        dto.DonosVinculados.Should().Contain(d => d.NomeAmigavel == "João Silva");
        dto.DonosVinculados.Should().Contain(d => d.NomeAmigavel == "Maria Silva");
    }

    [Fact]
    public void DeveMappearTamanhoFormatadoCorretamente()
    {
        // Arrange
        var documento = TestDataBuilders.CriarDocumento(
            tamanhoArquivo: 1536000L // 1.5 MB
        );

        // Act
        var dto = Mapper.Map<DocumentoDto>(documento);

        // Assert
        dto.Should().NotBeNull();
        dto.TamanhoFormatado.Should().Be(documento.ObterTamanhoFormatado());
        dto.TamanhoFormatado.Should().Contain("MB");
    }

    [Fact]
    public void DeveMappearTipoDocumentoNomeCorretamente()
    {
        // Arrange
        var idOrganizacao = CriarIdOrganizacao();
        var usuarioCriacao = CriarIdUsuario();
        var idTipoDocumento = CriarIdTipoDocumento();
        
        var tipoDocumento = TestDataBuilders.CriarTipoDocumento(
            id: idTipoDocumento,
            idOrganizacao: idOrganizacao,
            nome: "Carteira de Trabalho",
            usuarioCriacao: usuarioCriacao
        );

        var documento = TestDataBuilders.CriarDocumento(
            idOrganizacao: idOrganizacao,
            idTipoDocumento: idTipoDocumento,
            tipoDocumento: tipoDocumento,
            usuarioCriacao: usuarioCriacao
        );

        // Act
        var dto = Mapper.Map<DocumentoDto>(documento);

        // Assert
        dto.Should().NotBeNull();
        dto.TipoDocumentoNome.Should().Be("Carteira de Trabalho");
    }

    [Fact]
    public void DeveMappearStatusDocumentoCorreatamente()
    {
        // Arrange
        var usuarioAlteracao = CriarIdUsuario();
        var documento = TestDataBuilders.CriarDocumento(
            status: StatusDocumento.Inativo
        );

        // Act
        var dto = Mapper.Map<DocumentoDto>(documento);

        // Assert
        dto.Should().NotBeNull();
        dto.Status.Should().Be("Inativo");
    }

    [Fact]
    public void DeveMappearIdDocumentoCorretamente()
    {
        // Arrange
        var idEspecifico = CriarIdDocumento();
        var documento = TestDataBuilders.CriarDocumento(id: idEspecifico);

        // Act
        var dto = Mapper.Map<DocumentoDto>(documento);

        // Assert
        dto.Should().NotBeNull();
        dto.Id.Should().Be(idEspecifico.Valor);
    }

    [Fact]
    public void DeveMappearIdOrganizacaoCorretamente()
    {
        // Arrange
        var idOrganizacaoEspecifico = CriarIdOrganizacao();
        var documento = TestDataBuilders.CriarDocumento(idOrganizacao: idOrganizacaoEspecifico);

        // Act
        var dto = Mapper.Map<DocumentoDto>(documento);

        // Assert
        dto.Should().NotBeNull();
        dto.IdOrganizacao.Should().Be(idOrganizacaoEspecifico);
    }

    [Fact]
    public void DeveMappearIdTipoDocumentoCorretamente()
    {
        // Arrange
        var idTipoDocumentoEspecifico = CriarIdTipoDocumento();
        var documento = TestDataBuilders.CriarDocumento(idTipoDocumento: idTipoDocumentoEspecifico);

        // Act
        var dto = Mapper.Map<DocumentoDto>(documento);

        // Assert
        dto.Should().NotBeNull();
        dto.IdTipoDocumento.Should().Be(idTipoDocumentoEspecifico);
    }

    [Fact]
    public void DeveMappearNomeArquivoCorretamente()
    {
        // Arrange
        var nomeEspecifico = "documento_importante.docx";
        var documento = TestDataBuilders.CriarDocumento(nomeArquivo: nomeEspecifico);

        // Act
        var dto = Mapper.Map<DocumentoDto>(documento);

        // Assert
        dto.Should().NotBeNull();
        dto.NomeArquivo.Should().Be(nomeEspecifico);
    }

    [Fact]
    public void DeveMappearChaveArmazenamentoCorretamente()
    {
        // Arrange
        var chaveEspecifica = "/storage/2024/01/arquivo_123456.pdf";
        var documento = TestDataBuilders.CriarDocumento(chaveArmazenamento: chaveEspecifica);

        // Act
        var dto = Mapper.Map<DocumentoDto>(documento);

        // Assert
        dto.Should().NotBeNull();
        dto.ChaveArmazenamento.Should().Be(chaveEspecifica);
    }

    [Fact]
    public void DeveMappearDataUploadCorretamente()
    {
        // Arrange
        var dataEspecifica = DateTime.UtcNow.AddDays(-5);
        var documento = TestDataBuilders.CriarDocumento(dataUpload: dataEspecifica);

        // Act
        var dto = Mapper.Map<DocumentoDto>(documento);

        // Assert
        dto.Should().NotBeNull();
        dto.DataUpload.Should().Be(dataEspecifica);
    }

    [Fact]
    public void DeveMappearTamanhoArquivoCorretamente()
    {
        // Arrange
        var tamanhoEspecifico = 2048576L; // 2MB
        var documento = TestDataBuilders.CriarDocumento(tamanhoArquivo: tamanhoEspecifico);

        // Act
        var dto = Mapper.Map<DocumentoDto>(documento);

        // Assert
        dto.Should().NotBeNull();
        dto.TamanhoArquivo.Should().Be(tamanhoEspecifico);
    }

    [Fact]
    public void DeveMappearTipoArquivoCorretamente()
    {
        // Arrange
        var tipoEspecifico = "docx";
        var documento = TestDataBuilders.CriarDocumento(tipoArquivo: tipoEspecifico);

        // Act
        var dto = Mapper.Map<DocumentoDto>(documento);

        // Assert
        dto.Should().NotBeNull();
        dto.TipoArquivo.Should().Be(tipoEspecifico);
    }

    [Fact]
    public void DeveMappearVersaoCorretamente()
    {
        // Arrange
        var versaoEspecifica = 5;
        var documento = TestDataBuilders.CriarDocumento(versao: versaoEspecifica);

        // Act
        var dto = Mapper.Map<DocumentoDto>(documento);

        // Assert
        dto.Should().NotBeNull();
        dto.Versao.Should().Be(versaoEspecifica);
    }

    [Fact]
    public void DeveMappearCamposDeAuditoriaCorretamente()
    {
        // Arrange
        var usuarioCriacao = CriarIdUsuario();
        var documento = TestDataBuilders.CriarDocumento(usuarioCriacao: usuarioCriacao);

        // Act
        var dto = Mapper.Map<DocumentoDto>(documento);

        // Assert
        dto.Should().NotBeNull();
        dto.UsuarioCriacao.Should().Be(usuarioCriacao.Valor);
        dto.UsuarioUltimaAlteracao.Should().Be(usuarioCriacao.Valor);
        dto.DataCriacao.Should().Be(documento.DataCriacao);
        dto.DataUltimaAlteracao.Should().Be(documento.DataAtualizacao);
    }

    [Fact]
    public void DeveMappearListaDeDocumentos_ComSucesso()
    {
        // Arrange
        var idOrganizacao = CriarIdOrganizacao();
        var usuarioCriacao = CriarIdUsuario();
        var idTipoDocumento = CriarIdTipoDocumento();
        
        var tipoDocumento = TestDataBuilders.CriarTipoDocumento(
            id: idTipoDocumento,
            idOrganizacao: idOrganizacao,
            nome: "Foto",
            usuarioCriacao: usuarioCriacao
        );

        var documentos = new List<Documento>
        {
            TestDataBuilders.CriarDocumento(
                idOrganizacao: idOrganizacao,
                nomeArquivo: "foto1.jpg",
                idTipoDocumento: idTipoDocumento,
                tipoDocumento: tipoDocumento,
                usuarioCriacao: usuarioCriacao
            ),
            TestDataBuilders.CriarDocumento(
                idOrganizacao: idOrganizacao,
                nomeArquivo: "foto2.png",
                idTipoDocumento: idTipoDocumento,
                tipoDocumento: tipoDocumento,
                usuarioCriacao: usuarioCriacao
            ),
            TestDataBuilders.CriarDocumento(
                idOrganizacao: idOrganizacao,
                nomeArquivo: "foto3.gif",
                idTipoDocumento: idTipoDocumento,
                tipoDocumento: tipoDocumento,
                usuarioCriacao: usuarioCriacao
            )
        };

        // Act
        var dtos = Mapper.Map<List<DocumentoDto>>(documentos);

        // Assert
        dtos.Should().NotBeNull();
        dtos.Should().HaveCount(3);
        dtos[0].NomeArquivo.Should().Be("foto1.jpg");
        dtos[1].NomeArquivo.Should().Be("foto2.png");
        dtos[2].NomeArquivo.Should().Be("foto3.gif");
        dtos.Should().AllSatisfy(d => d.TipoDocumentoNome.Should().Be("Foto"));
    }

    [Fact]
    public void DeveMappearDocumentoSemDonosVinculados_ComListaVazia()
    {
        // Arrange
        var documento = TestDataBuilders.CriarDocumento(nomeArquivo: "documento_sem_donos.pdf");

        // Act
        var dto = Mapper.Map<DocumentoDto>(documento);

        // Assert
        dto.Should().NotBeNull();
        dto.DonosVinculados.Should().NotBeNull();
        dto.DonosVinculados.Should().BeEmpty();
    }

    [Theory]
    [InlineData(StatusDocumento.Ativo, "Ativo")]
    [InlineData(StatusDocumento.Inativo, "Inativo")]
    public void DeveMappearTodosOsStatusCorretamente(StatusDocumento status, string statusEsperado)
    {
        // Arrange
        var documento = TestDataBuilders.CriarDocumento(status: status);

        // Act
        var dto = Mapper.Map<DocumentoDto>(documento);

        // Assert
        dto.Should().NotBeNull();
        dto.Status.Should().Be(statusEsperado);
    }

    [Theory]
    [InlineData(1024L, "1 KB")]
    [InlineData(1048576L, "1 MB")]
    [InlineData(1073741824L, "1 GB")]
    public void DeveMappearTamanhoFormatado_ComDiferentesTamanhos(long tamanho, string tamanhoEsperadoAproximado)
    {
        // Arrange
        var documento = TestDataBuilders.CriarDocumento(tamanhoArquivo: tamanho);

        // Act
        var dto = Mapper.Map<DocumentoDto>(documento);

        // Assert
        dto.Should().NotBeNull();
        dto.TamanhoFormatado.Should().NotBeNullOrEmpty();
        // Verifica se contém a unidade esperada
        dto.TamanhoFormatado.Should().Contain(tamanhoEsperadoAproximado.Split(' ')[1]);
    }

    [Fact]
    public void NaoDeveGerarReferenciaCircular_QuandoMapearDonosVinculados()
    {
        // Arrange
        var idOrganizacao = CriarIdOrganizacao();
        var usuarioCriacao = CriarIdUsuario();
        var idTipoDocumento = CriarIdTipoDocumento();
        var idTipoDono = CriarIdTipoDono();
        
        var tipoDocumento = TestDataBuilders.CriarTipoDocumento(
            id: idTipoDocumento,
            idOrganizacao: idOrganizacao,
            usuarioCriacao: usuarioCriacao
        );

        var tipoDono = TestDataBuilders.CriarTipoDono(
            id: idTipoDono,
            idOrganizacao: idOrganizacao,
            usuarioCriacao: usuarioCriacao
        );

        tipoDocumento.VincularTipoDono(tipoDono);
        tipoDono.VincularTipoDocumento(tipoDocumento);

        var documento = TestDataBuilders.CriarDocumento(
            idOrganizacao: idOrganizacao,
            idTipoDocumento: idTipoDocumento,
            tipoDocumento: tipoDocumento,
            usuarioCriacao: usuarioCriacao
        );

        var donoDocumento = TestDataBuilders.CriarDonoDocumento(
            idOrganizacao: idOrganizacao,
            idTipoDono: idTipoDono,
            tipoDono: tipoDono,
            usuarioCriacao: usuarioCriacao
        );

        // Criar vinculação bidirecional
        documento.VincularDonoDocumento(donoDocumento);
        donoDocumento.VincularDocumento(documento);

        // Configurar propriedades de navegação
        var vinculacaoDocumento = documento.DonosVinculados.First();
        typeof(DocumentoDonoDocumento).GetProperty("DonoDocumento")?.SetValue(vinculacaoDocumento, donoDocumento);

        var vinculacaoDonoDocumento = donoDocumento.DocumentosVinculados.First();
        typeof(DocumentoDonoDocumento).GetProperty("Documento")?.SetValue(vinculacaoDonoDocumento, documento);

        // Act & Assert - Não deve gerar exceção de referência circular
        Action act = () => Mapper.Map<DocumentoDto>(documento);
        act.Should().NotThrow();

        var dto = Mapper.Map<DocumentoDto>(documento);
        dto.Should().NotBeNull();
        dto.DonosVinculados.Should().HaveCount(1);
    }
}