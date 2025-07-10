using DddBase.Base;
using Tsc.GestaoDocumentos.Domain.Entities;
using Tsc.GestaoDocumentos.Domain.Enums;
using Tsc.GestaoDocumentos.Domain.Usuarios;

namespace Tsc.GestaoDocumentos.Domain.Organizacoes;

public class Organizacao : EntidadeComAuditoria<IdOrganizacao>, IRaizAgregado
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

    protected Organizacao() : base() { }

    public Organizacao(string nomeOrganizacao, string slug, IdUsuario idUsuario)
        : base()
    {
        DefinirNomeOrganizacao(nomeOrganizacao);
        DefinirSlug(slug);
        Status = StatusTenant.Ativo;
        UsuarioCriacao = idUsuario;
        UsuarioUltimaAlteracao = idUsuario;
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

    public void AlterarStatus(StatusTenant novoStatus, IdUsuario usuarioAlteracao)
    {
        Status = novoStatus;
        UsuarioUltimaAlteracao = usuarioAlteracao;
    }

    public void DefinirDataExpiracao(DateTime? dataExpiracao, IdUsuario idUsuario)
    {
        if (dataExpiracao.HasValue && dataExpiracao.Value <= DateTime.UtcNow)
            throw new ArgumentException("Data de expiração deve ser futura", nameof(dataExpiracao));

        DataExpiracao = dataExpiracao;
        UsuarioUltimaAlteracao = idUsuario;
    }

    public void AdicionarUsuario(Usuario usuario)
    {
        if (usuario.IdOrganizacao != Id)
            throw new ArgumentException("Usuário deve pertencer ao mesmo tenant");

        _usuarios.Add(usuario);
    }

    public bool EstaAtivo() => Status == StatusTenant.Ativo;
    public bool EstaExpirado() => DataExpiracao.HasValue && DataExpiracao.Value <= DateTime.UtcNow;
}
