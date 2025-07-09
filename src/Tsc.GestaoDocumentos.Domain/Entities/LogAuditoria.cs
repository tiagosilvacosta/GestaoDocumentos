using Tsc.GestaoDocumentos.Domain.Common;
using Tsc.GestaoDocumentos.Domain.Enums;

namespace Tsc.GestaoDocumentos.Domain.Entities;

public class LogAuditoria : TenantEntity
{
    public Guid UsuarioId { get; private set; }
    public string EntidadeAfetada { get; private set; } = string.Empty;
    public Guid EntidadeId { get; private set; }
    public TipoOperacaoAuditoria Operacao { get; private set; }
    public string? DadosAnteriores { get; private set; }
    public string? DadosNovos { get; private set; }
    public DateTime DataHoraOperacao { get; private set; }
    public string IpUsuario { get; private set; } = string.Empty;
    public string? UserAgent { get; private set; }

    // Navegação
    public Tenant Tenant { get; private set; } = null!;
    public Usuario Usuario { get; private set; } = null!;

    protected LogAuditoria() : base() { }

    public LogAuditoria(
        Guid tenantId,
        Guid usuarioId,
        string entidadeAfetada,
        Guid entidadeId,
        TipoOperacaoAuditoria operacao,
        string ipUsuario,
        string? dadosAnteriores = null,
        string? dadosNovos = null,
        string? userAgent = null)
        : base(tenantId)
    {
        UsuarioId = usuarioId;
        DefinirEntidadeAfetada(entidadeAfetada);
        EntidadeId = entidadeId;
        Operacao = operacao;
        DefinirIpUsuario(ipUsuario);
        DadosAnteriores = dadosAnteriores;
        DadosNovos = dadosNovos;
        UserAgent = userAgent;
        DataHoraOperacao = DateTime.UtcNow;
        UsuarioCriacao = usuarioId;
        UsuarioUltimaAlteracao = usuarioId;
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
