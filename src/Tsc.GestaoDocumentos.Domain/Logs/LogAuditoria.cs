using DddBase.Base;
using Tsc.GestaoDocumentos.Domain.Organizacoes;
using Tsc.GestaoDocumentos.Domain.Usuarios;

namespace Tsc.GestaoDocumentos.Domain.Logs;


public enum TipoOperacaoAuditoria
{
    CREATE = 1,
    UPDATE = 2,
    DELETE = 3,
    DOWNLOAD = 4,
    LOGIN = 5,
    LOGOUT = 6
}

public class LogAuditoria : EntidadeComAuditoriaEOrganizacao<IdLogAuditoria>, IRaizAgregado
{
    public IdUsuario IdUsuario { get; private set; } = null!;
    public string EntidadeAfetada { get; private set; } = string.Empty;
    public Guid IdEntidade { get; private set; }
    public TipoOperacaoAuditoria Operacao { get; private set; }
    public string? DadosAnteriores { get; private set; }
    public string? DadosNovos { get; private set; }
    public DateTime DataHoraOperacao { get; private set; }
    public string IpUsuario { get; private set; } = string.Empty;
    public string? UserAgent { get; private set; }

    // Navegação
    public Organizacao Tenant { get; private set; } = null!;
    public Usuario Usuario { get; private set; } = null!;

    protected LogAuditoria() : base() { }

    public LogAuditoria(
        IdOrganizacao idOrganizacao,
        IdUsuario idUsuario,
        string entidadeAfetada,
        Guid entidadeId,
        TipoOperacaoAuditoria operacao,
        string ipUsuario,
        string? dadosAnteriores = null,
        string? dadosNovos = null,
        string? userAgent = null)
        : base(IdLogAuditoria.CriarNovo(), idOrganizacao)
    {
        IdUsuario = idUsuario;
        DefinirEntidadeAfetada(entidadeAfetada);
        IdEntidade = entidadeId;
        Operacao = operacao;
        DefinirIpUsuario(ipUsuario);
        DadosAnteriores = dadosAnteriores;
        DadosNovos = dadosNovos;
        UserAgent = userAgent;
        DataHoraOperacao = DateTime.UtcNow;
        UsuarioCriacao = idUsuario;
        UsuarioUltimaAlteracao = idUsuario;
    }

    public void DefinirEntidadeAfetada(string entidadeAfetada)
    {
        if (string.IsNullOrWhiteSpace(entidadeAfetada))
            throw new ArgumentException("Entidade afetada é obrigatória", nameof(entidadeAfetada));

        if (entidadeAfetada.Length > 100)
            throw new ArgumentException("Entidade afetada não pode ter mais de 100 caracteres", nameof(entidadeAfetada));

        EntidadeAfetada = entidadeAfetada.Trim();
    }

    public void DefinirIpUsuario(string ipUsuario)
    {
        if (string.IsNullOrWhiteSpace(ipUsuario))
            throw new ArgumentException("IP do usuário é obrigatório", nameof(ipUsuario));

        if (ipUsuario.Length > 45) // IPv6 pode ter até 45 caracteres
            throw new ArgumentException("IP do usuário não pode ter mais de 45 caracteres", nameof(ipUsuario));

        IpUsuario = ipUsuario.Trim();
    }
}
