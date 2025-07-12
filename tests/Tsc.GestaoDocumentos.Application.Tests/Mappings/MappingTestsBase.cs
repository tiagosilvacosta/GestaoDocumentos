using AutoMapper;
using Bogus;
using Tsc.GestaoDocumentos.Application.Mappings;
using Tsc.GestaoDocumentos.Domain.Documentos;
using Tsc.GestaoDocumentos.Domain.Organizacoes;
using Tsc.GestaoDocumentos.Domain.Usuarios;

namespace Tsc.GestaoDocumentos.Application.Tests.Mappings;

/// <summary>
/// Classe base para testes de mapeamento AutoMapper.
/// Fornece configuração comum e métodos auxiliares para criação de dados de teste.
/// </summary>
public abstract class MappingTestsBase
{
    protected readonly IMapper Mapper;
    protected readonly Faker Faker;

    protected MappingTestsBase()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<DomainToDtoProfile>();
        });

        configuration.AssertConfigurationIsValid();
        Mapper = configuration.CreateMapper();
        
        Faker = new Faker("pt_BR");
    }

    /// <summary>
    /// Cria um ID de organização para testes
    /// </summary>
    protected IdOrganizacao CriarIdOrganizacao() => IdOrganizacao.CriarNovo();

    /// <summary>
    /// Cria um ID de usuário para testes
    /// </summary>
    protected IdUsuario CriarIdUsuario() => IdUsuario.GerarNovo();

    /// <summary>
    /// Cria um ID de tipo de dono para testes
    /// </summary>
    protected IdTipoDono CriarIdTipoDono() => IdTipoDono.CriarNovo();

    /// <summary>
    /// Cria um ID de tipo de documento para testes
    /// </summary>
    protected IdTipoDocumento CriarIdTipoDocumento() => IdTipoDocumento.CriarNovo();

    /// <summary>
    /// Cria um ID de dono de documento para testes
    /// </summary>
    protected IdDonoDocumento CriarIdDonoDocumento() => IdDonoDocumento.CriarNovo();

    /// <summary>
    /// Cria um ID de documento para testes
    /// </summary>
    protected IdDocumento CriarIdDocumento() => IdDocumento.CriarNovo();

    /// <summary>
    /// Cria uma data aleatória no passado recente
    /// </summary>
    protected DateTime CriarDataAleatoria() => Faker.Date.Recent(30);

    /// <summary>
    /// Cria uma data aleatória de acesso
    /// </summary>
    protected DateTime? CriarDataAcessoAleatoria() => Faker.Random.Bool() ? Faker.Date.Recent(7) : null;

    /// <summary>
    /// Cria um nome amigável aleatório
    /// </summary>
    protected string CriarNomeAmigavel() => Faker.Person.FullName;

    /// <summary>
    /// Cria um nome de arquivo aleatório
    /// </summary>
    protected string CriarNomeArquivo() => Faker.System.FileName();

    /// <summary>
    /// Cria uma chave de armazenamento aleatória
    /// </summary>
    protected string CriarChaveArmazenamento() => Faker.System.FilePath();

    /// <summary>
    /// Cria um tamanho de arquivo aleatório
    /// </summary>
    protected long CriarTamanhoArquivo() => Faker.Random.Long(1000, 50000000);

    /// <summary>
    /// Cria um tipo de arquivo aleatório
    /// </summary>
    protected string CriarTipoArquivo() => Faker.PickRandom("pdf", "doc", "docx", "jpg", "png", "txt");

    /// <summary>
    /// Cria uma versão aleatória de documento
    /// </summary>
    protected int CriarVersao() => Faker.Random.Int(1, 10);
}