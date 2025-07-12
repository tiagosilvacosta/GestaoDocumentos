using Bogus;
using Tsc.GestaoDocumentos.Domain.Documentos;
using Tsc.GestaoDocumentos.Domain.Organizacoes;
using Tsc.GestaoDocumentos.Domain.Usuarios;

namespace Tsc.GestaoDocumentos.Application.Tests.Mappings.Helpers;

/// <summary>
/// Classe auxiliar para criação de entidades de teste usando Bogus.
/// </summary>
public static class TestDataBuilders
{
    private static readonly Faker Faker = new("pt_BR");

    /// <summary>
    /// Cria uma organização para testes
    /// </summary>
    public static Organizacao CriarOrganizacao(
        IdOrganizacao? id = null,
        string? nomeOrganizacao = null,
        string? slug = null,
        StatusTenant? status = null,
        DateTime? dataExpiracao = null,
        IdUsuario? usuarioCriacao = null,
        IdUsuario? usuarioUltimaAlteracao = null)
    {
        var idUsuario = usuarioCriacao ?? IdUsuario.GerarNovo();
        var organizacao = new Organizacao(
            nomeOrganizacao ?? Faker.Company.CompanyName(),
            slug?.ToLowerInvariant() ?? Faker.Random.AlphaNumeric(8).ToLowerInvariant(),
            idUsuario
        );

        if (id != null)
            typeof(Organizacao).GetProperty("Id")?.SetValue(organizacao, id);

        if (status.HasValue)
            organizacao.AlterarStatus(status.Value, usuarioUltimaAlteracao ?? idUsuario);

        if (dataExpiracao.HasValue)
            organizacao.DefinirDataExpiracao(dataExpiracao, usuarioUltimaAlteracao ?? idUsuario);

        if (usuarioUltimaAlteracao != null)
            typeof(Organizacao).GetProperty("UsuarioUltimaAlteracao")?.SetValue(organizacao, usuarioUltimaAlteracao);

        return organizacao;
    }

    /// <summary>
    /// Cria um usuário para testes
    /// </summary>
    public static Usuario CriarUsuario(
        IdUsuario? id = null,
        IdOrganizacao? idOrganizacao = null,
        string? nome = null,
        string? email = null,
        string? login = null,
        string? senhaHash = null,
        PerfilUsuario? perfil = null,
        StatusUsuario? status = null,
        DateTime? ultimoAcesso = null,
        IdUsuario? usuarioCriacao = null,
        IdUsuario? usuarioUltimaAlteracao = null)
    {
        var idUsuarioCriacao = usuarioCriacao ?? IdUsuario.GerarNovo();
        var usuario = new Usuario(
            idOrganizacao ?? IdOrganizacao.CriarNovo(),
            nome ?? Faker.Person.FullName,
            email ?? Faker.Person.Email,
            login ?? Faker.Internet.UserName(),
            senhaHash ?? Faker.Random.AlphaNumeric(60),
            perfil ?? Faker.PickRandom<PerfilUsuario>(),
            idUsuarioCriacao
        );

        if (id != null)
            typeof(Usuario).GetProperty("Id")?.SetValue(usuario, id);

        if (status.HasValue)
            usuario.AlterarStatus(status.Value, usuarioUltimaAlteracao ?? idUsuarioCriacao);

        if (usuarioUltimaAlteracao != null)
            typeof(Usuario).GetProperty("UsuarioUltimaAlteracao")?.SetValue(usuario, usuarioUltimaAlteracao);

        if (ultimoAcesso.HasValue)
        {
            usuario.RegistrarAcesso();
            typeof(Usuario).GetProperty("UltimoAcesso")?.SetValue(usuario, ultimoAcesso.Value);
        }

        return usuario;
    }

    /// <summary>
    /// Cria um tipo de dono para testes
    /// </summary>
    public static TipoDono CriarTipoDono(
        IdTipoDono? id = null,
        IdOrganizacao? idOrganizacao = null,
        string? nome = null,
        IdUsuario? usuarioCriacao = null)
    {
        var tipoDono = new TipoDono(
            idOrganizacao ?? IdOrganizacao.CriarNovo(),
            nome ?? Faker.PickRandom("Pessoa Física", "Pessoa Jurídica", "Entidade Governamental"),
            usuarioCriacao ?? IdUsuario.GerarNovo()
        );

        if (id != null)
            typeof(TipoDono).GetProperty("Id")?.SetValue(tipoDono, id);

        return tipoDono;
    }

    /// <summary>
    /// Cria um tipo de documento para testes
    /// </summary>
    public static TipoDocumento CriarTipoDocumento(
        IdTipoDocumento? id = null,
        IdOrganizacao? idOrganizacao = null,
        string? nome = null,
        bool? permiteMultiplosDocumentos = null,
        IdUsuario? usuarioCriacao = null)
    {
        var tipoDocumento = new TipoDocumento(
            idOrganizacao ?? IdOrganizacao.CriarNovo(),
            nome ?? Faker.PickRandom("CPF", "RG", "CNH", "Passaporte", "Nota Fiscal"),
            permiteMultiplosDocumentos ?? Faker.Random.Bool(),
            usuarioCriacao ?? IdUsuario.GerarNovo()
        );

        if (id != null)
            typeof(TipoDocumento).GetProperty("Id")?.SetValue(tipoDocumento, id);

        return tipoDocumento;
    }

    /// <summary>
    /// Cria um dono de documento para testes
    /// </summary>
    public static DonoDocumento CriarDonoDocumento(
        IdDonoDocumento? id = null,
        IdOrganizacao? idOrganizacao = null,
        string? nomeAmigavel = null,
        IdTipoDono? idTipoDono = null,
        TipoDono? tipoDono = null,
        IdUsuario? usuarioCriacao = null)
    {
        var donoDocumento = new DonoDocumento(
            idOrganizacao ?? IdOrganizacao.CriarNovo(),
            nomeAmigavel ?? Faker.Person.FullName,
            idTipoDono ?? IdTipoDono.CriarNovo(),
            usuarioCriacao ?? IdUsuario.GerarNovo()
        );

        if (id != null)
            typeof(DonoDocumento).GetProperty("Id")?.SetValue(donoDocumento, id);

        // Se um TipoDono foi fornecido, definir a propriedade de navegação
        if (tipoDono != null)
            typeof(DonoDocumento).GetProperty("TipoDono")?.SetValue(donoDocumento, tipoDono);

        return donoDocumento;
    }

    /// <summary>
    /// Cria um documento para testes
    /// </summary>
    public static Documento CriarDocumento(
        IdDocumento? id = null,
        IdOrganizacao? idOrganizacao = null,
        string? nomeArquivo = null,
        string? chaveArmazenamento = null,
        DateTime? dataUpload = null,
        long? tamanhoArquivo = null,
        string? tipoArquivo = null,
        int? versao = null,
        StatusDocumento? status = null,
        IdTipoDocumento? idTipoDocumento = null,
        TipoDocumento? tipoDocumento = null,
        IdUsuario? usuarioCriacao = null)
    {
        var documento = new Documento(
            idOrganizacao ?? IdOrganizacao.CriarNovo(),
            nomeArquivo ?? Faker.System.FileName(),
            chaveArmazenamento ?? Faker.System.FilePath(),
            tamanhoArquivo ?? Faker.Random.Long(1000, 50000000),
            tipoArquivo ?? Faker.PickRandom("pdf", "doc", "docx", "jpg", "png"),
            idTipoDocumento ?? IdTipoDocumento.CriarNovo(),
            usuarioCriacao ?? IdUsuario.GerarNovo()
        );

        if (id != null)
            typeof(Documento).GetProperty("Id")?.SetValue(documento, id);

        if (dataUpload.HasValue)
            typeof(Documento).GetProperty("DataUpload")?.SetValue(documento, dataUpload.Value);

        if (versao.HasValue)
            documento.DefinirVersao(versao.Value);

        if (status.HasValue)
            documento.AlterarStatus(status.Value, usuarioCriacao ?? IdUsuario.GerarNovo());

        // Se um TipoDocumento foi fornecido, definir a propriedade de navegação
        if (tipoDocumento != null)
            typeof(Documento).GetProperty("TipoDocumento")?.SetValue(documento, tipoDocumento);

        return documento;
    }
}