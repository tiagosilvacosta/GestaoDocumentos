using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Tsc.GestaoDocumentos.Infrastructure.Data;
using Tsc.GestaoDocumentos.Infrastructure.Usuarios;
using Xunit;

namespace Tsc.GestaoDocumentos.Infrastructure.Tests.Services;

public class CriptografiaServiceTests
{
    private readonly ServicoCriptografia _service;

    public CriptografiaServiceTests()
    {
        _service = new ServicoCriptografia();
    }

    [Fact]
    public void GerarHashSenha_DeveRetornarHashValido()
    {
        // Arrange
        var senha = "MinhaSenh@123";

        // Act
        var hash = _service.GerarHashSenha(senha);

        // Assert
        hash.Should().NotBeNullOrEmpty();
        hash.Should().NotBe(senha);
    }

    [Fact]
    public void VerificarSenha_DeveFuncionar()
    {
        // Arrange
        var senha = "MinhaSenh@123";
        var hash = _service.GerarHashSenha(senha);

        // Act
        var resultado = _service.VerificarSenha(senha, hash);

        // Assert
        resultado.Should().BeTrue();
    }

    [Fact]
    public void VerificarSenha_DeveRejeitarSenhaIncorreta()
    {
        // Arrange
        var senhaCorreta = "MinhaSenh@123";
        var senhaIncorreta = "SenhaErrada";
        var hash = _service.GerarHashSenha(senhaCorreta);

        // Act
        var resultado = _service.VerificarSenha(senhaIncorreta, hash);

        // Assert
        resultado.Should().BeFalse();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void GerarHashSenha_DeveRejeitarSenhaInvalida(string senhaInvalida)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => _service.GerarHashSenha(senhaInvalida));
    }

    [Fact]
    public void GerarSalt_DeveRetornarSaltValido()
    {
        // Act
        var salt = _service.GerarSalt();

        // Assert
        salt.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void HashesIguais_DevemTerSaltsDiferentes()
    {
        // Arrange
        var senha = "MinhaSenh@123";

        // Act
        var hash1 = _service.GerarHashSenha(senha);
        var hash2 = _service.GerarHashSenha(senha);

        // Assert
        hash1.Should().NotBe(hash2);
        _service.VerificarSenha(senha, hash1).Should().BeTrue();
        _service.VerificarSenha(senha, hash2).Should().BeTrue();
    }
}

public sealed class DbContextTests : IDisposable
{
    private readonly GestaoDocumentosDbContext _context;

    public DbContextTests()
    {
        var options = new DbContextOptionsBuilder<GestaoDocumentosDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new GestaoDocumentosDbContext(options);
    }

    [Fact]
    public void DbContext_DeveTerTodasAsEntidades()
    {
        // Act & Assert
        _context.Tenants.Should().NotBeNull();
        _context.Usuarios.Should().NotBeNull();
        _context.TiposDono.Should().NotBeNull();
        _context.TiposDocumento.Should().NotBeNull();
        _context.DonosDocumento.Should().NotBeNull();
        _context.Documentos.Should().NotBeNull();
        _context.LogsAuditoria.Should().NotBeNull();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
