using System.Security.Cryptography;
using System.Text;
using Tsc.GestaoDocumentos.Domain.Usuarios;

namespace Tsc.GestaoDocumentos.Infrastructure.Services;

public class CriptografiaService : ICriptografiaService
{
    private const int SaltSize = 32;
    private const int HashSize = 32;
    private const int Iterations = 100000;

    public string GerarHashSenha(string senha)
    {
        if (string.IsNullOrWhiteSpace(senha))
            throw new ArgumentException("Senha não pode ser vazia", nameof(senha));

        var salt = GerarSaltBytes();
        var hash = GerarHash(senha, salt);

        return Convert.ToBase64String(CombinarSaltEHash(salt, hash));
    }

    public bool VerificarSenha(string senha, string hash)
    {
        if (string.IsNullOrWhiteSpace(senha) || string.IsNullOrWhiteSpace(hash))
            return false;

        try
        {
            var hashBytes = Convert.FromBase64String(hash);
            var (salt, storedHash) = ExtrairSaltEHash(hashBytes);
            var computedHash = GerarHash(senha, salt);

            return CryptographicOperations.FixedTimeEquals(storedHash, computedHash);
        }
        catch
        {
            return false;
        }
    }

    public string GerarSalt()
    {
        return Convert.ToBase64String(GerarSaltBytes());
    }

    private byte[] GerarSaltBytes()
    {
        using var rng = RandomNumberGenerator.Create();
        var salt = new byte[SaltSize];
        rng.GetBytes(salt);
        return salt;
    }

    private byte[] GerarHash(string senha, byte[] salt)
    {
        using var pbkdf2 = new Rfc2898DeriveBytes(
            Encoding.UTF8.GetBytes(senha),
            salt,
            Iterations,
            HashAlgorithmName.SHA256);

        return pbkdf2.GetBytes(HashSize);
    }

    private byte[] CombinarSaltEHash(byte[] salt, byte[] hash)
    {
        var combined = new byte[SaltSize + HashSize];
        Array.Copy(salt, 0, combined, 0, SaltSize);
        Array.Copy(hash, 0, combined, SaltSize, HashSize);
        return combined;
    }

    private (byte[] salt, byte[] hash) ExtrairSaltEHash(byte[] combined)
    {
        if (combined.Length != SaltSize + HashSize)
            throw new ArgumentException("Hash inválido");

        var salt = new byte[SaltSize];
        var hash = new byte[HashSize];

        Array.Copy(combined, 0, salt, 0, SaltSize);
        Array.Copy(combined, SaltSize, hash, 0, HashSize);

        return (salt, hash);
    }
}
