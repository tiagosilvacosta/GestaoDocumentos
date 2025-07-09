using Tsc.GestaoDocumentos.Domain.Common;
using Tsc.GestaoDocumentos.Domain.Enums;

namespace Tsc.GestaoDocumentos.Domain.Entities;

public class Tenant : Entity
{
    public string NomeOrganizacao { get; private set; } = string.Empty;
    public string Slug { get; private set; } = string.Empty;
    public StatusTenant Status { get; private set; }
    public DateTime? DataExpiracao { get; private set; }

    // Navegação
    private readonly List<Usuario> _usuarios = new();
    public IReadOnlyCollection<Usuario> Usuarios => _usuarios.AsReadOnly();

    private readonly List<TipoDono> _tiposDono = new();
    public IReadOnlyCollection<TipoDono> TiposDono => _tiposDono.AsReadOnly();

    private readonly List<TipoDocumento> _tiposDocumento = new();
    public IReadOnlyCollection<TipoDocumento> TiposDocumento => _tiposDocumento.AsReadOnly();

    private readonly List<DonoDocumento> _donosDocumento = new();
    public IReadOnlyCollection<DonoDocumento> DonosDocumento => _donosDocumento.AsReadOnly();

    protected Tenant() : base() { }

    public Tenant(string nomeOrganizacao, string slug, Guid usuarioCriacao)
        : base()
    {
        DefinirNomeOrganizacao(nomeOrganizacao);
        DefinirSlug(slug);
        Status = StatusTenant.Ativo;
        UsuarioCriacao = usuarioCriacao;
        UsuarioUltimaAlteracao = usuarioCriacao;
    }

    public void DefinirNomeOrganizacao(string nomeOrganizacao)
    {
        if (string.IsNullOrWhiteSpace(nomeOrganizacao))
            throw new ArgumentException("Nome da organização é obrigatório", nameof(nomeOrganizacao));

        if (nomeOrganizacao.Length > 255)
            throw new ArgumentException("Nome da organização não pode ter mais de 255 caracteres", nameof(nomeOrganizacao));

        NomeOrganizacao = nomeOrganizacao.Trim();
    }

    public void DefinirSlug(string slug)
    {
        if (string.IsNullOrWhiteSpace(slug))
            throw new ArgumentException("Slug é obrigatório", nameof(slug));

        if (slug.Length > 50)
            throw new ArgumentException("Slug não pode ter mais de 50 caracteres", nameof(slug));

        if (!System.Text.RegularExpressions.Regex.IsMatch(slug, @"^[a-z0-9-]+$"))
            throw new ArgumentException("Slug deve conter apenas letras minúsculas, números e hífens", nameof(slug));

        Slug = slug.Trim().ToLowerInvariant();
    }

    public void AlterarStatus(StatusTenant novoStatus, Guid usuarioAlteracao)
    {
        Status = novoStatus;
        AtualizarDataAlteracao(usuarioAlteracao);
    }

    public void DefinirDataExpiracao(DateTime? dataExpiracao, Guid usuarioAlteracao)
    {
        if (dataExpiracao.HasValue && dataExpiracao.Value <= DateTime.UtcNow)
            throw new ArgumentException("Data de expiração deve ser futura", nameof(dataExpiracao));

        DataExpiracao = dataExpiracao;
        AtualizarDataAlteracao(usuarioAlteracao);
    }

    public void AdicionarUsuario(Usuario usuario)
    {
        if (usuario.TenantId != Id)
            throw new ArgumentException("Usuário deve pertencer ao mesmo tenant");

        _usuarios.Add(usuario);
    }

    public bool EstaAtivo() => Status == StatusTenant.Ativo;
    public bool EstaExpirado() => DataExpiracao.HasValue && DataExpiracao.Value <= DateTime.UtcNow;
}
