using FluentAssertions;
using Tsc.GestaoDocumentos.Infrastructure.Usuarios;
using Xunit;

namespace Tsc.GestaoDocumentos.Infrastructure.Tests.Usuarios;

public class ServicoCriptografiaTests
{
    private readonly ServicoCriptografia _servicoCriptografia;

    public ServicoCriptografiaTests()
    {
        _servicoCriptografia = new ServicoCriptografia();
    }

    [Fact]
    public void GerarHashSenha_DeveGerarHashValidoParaSenhaValida()
    {
        // Arrange
        var senha = "MinhaSenh@123";

        // Act
        var hash = _servicoCriptografia.GerarHashSenha(senha);

        // Assert
        hash.Should().NotBeNullOrEmpty();
        hash.Should().MatchRegex(@"^[A-Za-z0-9+/]*={0,2}$"); // Formato Base64
    }

    [Fact]
    public void GerarHashSenha_DeveGerarHashesDiferentesParaSenhasDiferentes()
    {
        // Arrange
        var senha1 = "MinhaSenh@123";
        var senha2 = "OutraSenha456";

        // Act
        var hash1 = _servicoCriptografia.GerarHashSenha(senha1);
        var hash2 = _servicoCriptografia.GerarHashSenha(senha2);

        // Assert
        hash1.Should().NotBe(hash2);
    }

    [Fact]
    public void GerarHashSenha_DeveGerarHashesDiferentesParaMesmaSenha()
    {
        // Arrange
        var senha = "MinhaSenh@123";

        // Act
        var hash1 = _servicoCriptografia.GerarHashSenha(senha);
        var hash2 = _servicoCriptografia.GerarHashSenha(senha);

        // Assert
        hash1.Should().NotBe(hash2, "devido ao salt diferente");
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void GerarHashSenha_DeveRejeitarSenhaInvalida(string? senhaInvalida)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => 
            _servicoCriptografia.GerarHashSenha(senhaInvalida!));
    }

    [Fact]
    public void GerarHashSenha_DeveGerarHashEmFormatoBase64Valido()
    {
        // Arrange
        var senha = "MinhaSenh@123";

        // Act
        var hash = _servicoCriptografia.GerarHashSenha(senha);

        // Assert
        var action = () => Convert.FromBase64String(hash);
        action.Should().NotThrow("hash deve ser Base64 válido");
    }

    [Fact]
    public void VerificarSenha_DeveRetornarTrueParaSenhaCorreta()
    {
        // Arrange
        var senha = "MinhaSenh@123";
        var hash = _servicoCriptografia.GerarHashSenha(senha);

        // Act
        var resultado = _servicoCriptografia.VerificarSenha(senha, hash);

        // Assert
        resultado.Should().BeTrue();
    }

    [Fact]
    public void VerificarSenha_DeveRetornarFalseParaSenhaIncorreta()
    {
        // Arrange
        var senhaOriginal = "MinhaSenh@123";
        var senhaIncorreta = "SenhaErrada456";
        var hash = _servicoCriptografia.GerarHashSenha(senhaOriginal);

        // Act
        var resultado = _servicoCriptografia.VerificarSenha(senhaIncorreta, hash);

        // Assert
        resultado.Should().BeFalse();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void VerificarSenha_DeveRetornarFalseParaSenhaVazia(string? senhaVazia)
    {
        // Arrange
        var hashValido = _servicoCriptografia.GerarHashSenha("SenhaValida123");

        // Act
        var resultado = _servicoCriptografia.VerificarSenha(senhaVazia!, hashValido);

        // Assert
        resultado.Should().BeFalse();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void VerificarSenha_DeveRetornarFalseParaHashVazio(string? hashVazio)
    {
        // Arrange
        var senha = "MinhaSenh@123";

        // Act
        var resultado = _servicoCriptografia.VerificarSenha(senha, hashVazio!);

        // Assert
        resultado.Should().BeFalse();
    }

    [Fact]
    public void VerificarSenha_DeveRetornarFalseParaHashInvalido()
    {
        // Arrange
        var senha = "MinhaSenh@123";
        var hashInvalido = "hash_invalido_nao_base64";

        // Act
        var resultado = _servicoCriptografia.VerificarSenha(senha, hashInvalido);

        // Assert
        resultado.Should().BeFalse();
    }

    [Fact]
    public void VerificarSenha_DeveRetornarFalseParaHashComFormatoIncorreto()
    {
        // Arrange
        var senha = "MinhaSenh@123";
        var hashComFormatoIncorreto = Convert.ToBase64String(new byte[10]); // Muito pequeno

        // Act
        var resultado = _servicoCriptografia.VerificarSenha(senha, hashComFormatoIncorreto);

        // Assert
        resultado.Should().BeFalse();
    }

    [Fact]
    public void VerificarSenha_DeveSerCaseSensitive()
    {
        // Arrange
        var senhaOriginal = "MinhaSenh@123";
        var senhaComCasoDiferente = "minhaSenh@123";
        var hash = _servicoCriptografia.GerarHashSenha(senhaOriginal);

        // Act
        var resultado = _servicoCriptografia.VerificarSenha(senhaComCasoDiferente, hash);

        // Assert
        resultado.Should().BeFalse();
    }

    [Fact]
    public void GerarSalt_DeveGerarSaltValido()
    {
        // Act
        var salt = _servicoCriptografia.GerarSalt();

        // Assert
        salt.Should().NotBeNullOrEmpty();
        salt.Should().MatchRegex(@"^[A-Za-z0-9+/]*={0,2}$"); // Formato Base64
    }

    [Fact]
    public void GerarSalt_DeveGerarSaltsDiferentesACadaChamada()
    {
        // Act
        var salt1 = _servicoCriptografia.GerarSalt();
        var salt2 = _servicoCriptografia.GerarSalt();

        // Assert
        salt1.Should().NotBe(salt2);
    }

    [Fact]
    public void GerarSalt_DeveGerarSaltEmFormatoBase64Valido()
    {
        // Act
        var salt = _servicoCriptografia.GerarSalt();

        // Assert
        var action = () => Convert.FromBase64String(salt);
        action.Should().NotThrow("salt deve ser Base64 válido");
    }

    [Fact]
    public void GerarSalt_DeveGerarSaltComTamanhoCorreto()
    {
        // Act
        var salt = _servicoCriptografia.GerarSalt();
        var saltBytes = Convert.FromBase64String(salt);

        // Assert
        saltBytes.Length.Should().Be(32, "tamanho do salt deve ser 32 bytes");
    }

    [Fact]
    public void CicloCompleto_DeveValidarGerarHashEVerificarSenha()
    {
        // Arrange
        var senha = "MinhaSenh@123";

        // Act
        var hash = _servicoCriptografia.GerarHashSenha(senha);
        var verificacao = _servicoCriptografia.VerificarSenha(senha, hash);

        // Assert
        verificacao.Should().BeTrue();
    }

    [Fact]
    public void MultiplasVerificacoes_DeveMantarConsistencia()
    {
        // Arrange
        var senha = "MinhaSenh@123";
        var hash = _servicoCriptografia.GerarHashSenha(senha);

        // Act & Assert
        for (int i = 0; i < 10; i++)
        {
            var resultado = _servicoCriptografia.VerificarSenha(senha, hash);
            resultado.Should().BeTrue($"verificação {i + 1} deve ser verdadeira");
        }
    }

    [Fact]
    public void DiferentesInstancias_DeveMantarConsistencia()
    {
        // Arrange
        var servico1 = new ServicoCriptografia();
        var servico2 = new ServicoCriptografia();
        var senha = "MinhaSenh@123";

        // Act
        var hash = servico1.GerarHashSenha(senha);
        var verificacao = servico2.VerificarSenha(senha, hash);

        // Assert
        verificacao.Should().BeTrue("diferentes instâncias devem ser compatíveis");
    }

    [Fact]
    public void GerarHashSenha_DeveUsarNumeroAdequadoDeIteracoes()
    {
        // Arrange
        var senha = "MinhaSenh@123";
        var inicioTempo = DateTime.UtcNow;

        // Act
        var hash = _servicoCriptografia.GerarHashSenha(senha);
        var tempoDecorrido = DateTime.UtcNow - inicioTempo;

        // Assert
        hash.Should().NotBeNullOrEmpty();
        tempoDecorrido.Should().BeGreaterThan(TimeSpan.FromMilliseconds(10), 
            "deve haver tempo mínimo para iterações PBKDF2");
        tempoDecorrido.Should().BeLessThan(TimeSpan.FromSeconds(2), 
            "não deve ser muito lento para uso prático");
    }

    [Fact]
    public void VerificarSenha_DeveUsarTempoConstante()
    {
        // Arrange
        var senha = "MinhaSenh@123";
        var hash = _servicoCriptografia.GerarHashSenha(senha);
        var senhaIncorreta = "SenhaErrada456";

        // Act - Múltiplas verificações para medir tempo
        var tempos = new List<TimeSpan>();
        
        for (int i = 0; i < 5; i++)
        {
            var inicio = DateTime.UtcNow;
            _servicoCriptografia.VerificarSenha(senha, hash);
            tempos.Add(DateTime.UtcNow - inicio);
        }

        for (int i = 0; i < 5; i++)
        {
            var inicio = DateTime.UtcNow;
            _servicoCriptografia.VerificarSenha(senhaIncorreta, hash);
            tempos.Add(DateTime.UtcNow - inicio);
        }

        // Assert
        var tempoMedio = TimeSpan.FromTicks((long)tempos.Average(t => t.Ticks));
        var desvio = tempos.Max(t => Math.Abs((t - tempoMedio).TotalMilliseconds));
        
        desvio.Should().BeLessThan(100, "variação de tempo deve ser pequena para evitar timing attacks");
    }
}
