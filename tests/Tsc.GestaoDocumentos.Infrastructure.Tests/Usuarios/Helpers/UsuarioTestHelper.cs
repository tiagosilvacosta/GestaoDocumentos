using Bogus;
using Microsoft.EntityFrameworkCore;
using Tsc.GestaoDocumentos.Domain.Organizacoes;
using Tsc.GestaoDocumentos.Domain.Usuarios;
using Tsc.GestaoDocumentos.Infrastructure.Data;

namespace Tsc.GestaoDocumentos.Infrastructure.Tests.Usuarios.Helpers;

public static class UsuarioTestHelper
{
    public static Usuario CriarUsuarioValido(IdOrganizacao? idOrganizacao = null)
    {
        var organizacao = idOrganizacao ?? IdOrganizacao.CriarNovo();
        var faker = new Faker("pt_BR");
        
        return new Usuario(
            organizacao,
            faker.Name.FullName(),
            faker.Internet.Email().ToLowerInvariant(),
            faker.Internet.UserName().ToLowerInvariant(),
            faker.Internet.Password(),
            PerfilUsuario.Usuario,
            IdUsuario.GerarNovo());
    }

    public static Usuario CriarUsuarioComOrganizacao(IdOrganizacao idOrganizacao)
    {
        return CriarUsuarioValido(idOrganizacao);
    }

    public static Usuario CriarUsuarioComPerfil(PerfilUsuario perfil, IdOrganizacao? idOrganizacao = null)
    {
        var organizacao = idOrganizacao ?? IdOrganizacao.CriarNovo();
        var faker = new Faker("pt_BR");
        
        return new Usuario(
            organizacao,
            faker.Name.FullName(),
            faker.Internet.Email().ToLowerInvariant(),
            faker.Internet.UserName().ToLowerInvariant(),
            faker.Internet.Password(),
            perfil,
            IdUsuario.GerarNovo());
    }

    public static Usuario CriarUsuarioComEmailELogin(string email, string login, IdOrganizacao? idOrganizacao = null)
    {
        var organizacao = idOrganizacao ?? IdOrganizacao.CriarNovo();
        var faker = new Faker("pt_BR");
        
        return new Usuario(
            organizacao,
            faker.Name.FullName(),
            email.ToLowerInvariant(),
            login.ToLowerInvariant(),
            faker.Internet.Password(),
            PerfilUsuario.Usuario,
            IdUsuario.GerarNovo());
    }

    public static List<Usuario> CriarListaUsuarios(int quantidade, IdOrganizacao? idOrganizacao = null)
    {
        var organizacao = idOrganizacao ?? IdOrganizacao.CriarNovo();
        var usuarios = new List<Usuario>();
        
        for (int i = 0; i < quantidade; i++)
        {
            usuarios.Add(CriarUsuarioValido(organizacao));
        }
        
        return usuarios;
    }

    public static GestaoDocumentosDbContext CriarDbContextEmMemoria(string? nomeDatabase = null)
    {
        var nomeBanco = nomeDatabase ?? Guid.NewGuid().ToString();
        var options = new DbContextOptionsBuilder<GestaoDocumentosDbContext>()
            .UseInMemoryDatabase(databaseName: nomeBanco)
            .Options;

        return new GestaoDocumentosDbContext(options);
    }

    public static async Task<GestaoDocumentosDbContext> CriarDbContextComUsuarios(List<Usuario> usuarios)
    {
        var context = CriarDbContextEmMemoria();
        
        await context.Usuarios.AddRangeAsync(usuarios);
        await context.SaveChangesAsync();
        
        return context;
    }
}
