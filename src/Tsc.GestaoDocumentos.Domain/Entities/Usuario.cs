using Tsc.GestaoDocumentos.Domain.Common;
using Tsc.GestaoDocumentos.Domain.Enums;

namespace Tsc.GestaoDocumentos.Domain.Entities;

public class Usuario : TenantEntity
{
    public string Nome { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string Login { get; private set; } = string.Empty;
    public string SenhaHash { get; private set; } = string.Empty;
    public StatusUsuario Status { get; private set; }
    public PerfilUsuario Perfil { get; private set; }
    public DateTime? UltimoAcesso { get; private set; }

    // Navegação
    public Tenant Tenant { get; private set; } = null!;

    protected Usuario() : base() { }

    public Usuario(
        Guid tenantId,
        string nome,
        string email,
        string login,
        string senhaHash,
        PerfilUsuario perfil,
        Guid usuarioCriacao)
        : base(tenantId)
    {
        DefinirNome(nome);
        DefinirEmail(email);
        DefinirLogin(login);
        DefinirSenhaHash(senhaHash);
        Perfil = perfil;
        Status = StatusUsuario.Ativo;
        UsuarioCriacao = usuarioCriacao;
        UsuarioUltimaAlteracao = usuarioCriacao;
    }

    public void DefinirNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome é obrigatório", nameof(nome));

        if (nome.Length > 255)
            throw new ArgumentException("Nome não pode ter mais de 255 caracteres", nameof(nome));

        Nome = nome.Trim();
    }

    public void DefinirEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email é obrigatório", nameof(email));

        if (email.Length > 255)
            throw new ArgumentException("Email não pode ter mais de 255 caracteres", nameof(email));

        if (!IsValidEmail(email))
            throw new ArgumentException("Email deve ter um formato válido", nameof(email));

        Email = email.Trim().ToLowerInvariant();
    }

    public void DefinirLogin(string login)
    {
        if (string.IsNullOrWhiteSpace(login))
            throw new ArgumentException("Login é obrigatório", nameof(login));

        if (login.Length > 100)
            throw new ArgumentException("Login não pode ter mais de 100 caracteres", nameof(login));

        Login = login.Trim().ToLowerInvariant();
    }

    public void DefinirSenhaHash(string senhaHash)
    {
        if (string.IsNullOrWhiteSpace(senhaHash))
            throw new ArgumentException("Hash da senha é obrigatório", nameof(senhaHash));

        SenhaHash = senhaHash;
    }

    public void AlterarStatus(StatusUsuario novoStatus, Guid usuarioAlteracao)
    {
        Status = novoStatus;
        AtualizarDataAlteracao(usuarioAlteracao);
    }

    public void AlterarPerfil(PerfilUsuario novoPerfil, Guid usuarioAlteracao)
    {
        Perfil = novoPerfil;
        AtualizarDataAlteracao(usuarioAlteracao);
    }

    public void RegistrarAcesso()
    {
        UltimoAcesso = DateTime.UtcNow;
    }

    public bool EstaAtivo() => Status == StatusUsuario.Ativo;

    private static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
}
