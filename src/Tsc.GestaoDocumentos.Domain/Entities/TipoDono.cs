using Tsc.GestaoDocumentos.Domain.Common;

namespace Tsc.GestaoDocumentos.Domain.Entities;

public class TipoDono : TenantEntity
{
    public string Nome { get; private set; } = string.Empty;

    // Navegação
    public Tenant Tenant { get; private set; } = null!;
    
    private readonly List<DonoDocumento> _donosDocumento = new();
    public IReadOnlyCollection<DonoDocumento> DonosDocumento => _donosDocumento.AsReadOnly();

    private readonly List<TipoDonoTipoDocumento> _tiposDocumentoVinculados = new();
    public IReadOnlyCollection<TipoDonoTipoDocumento> TiposDocumentoVinculados => _tiposDocumentoVinculados.AsReadOnly();

    protected TipoDono() : base() { }

    public TipoDono(Guid tenantId, string nome, Guid usuarioCriacao)
        : base(tenantId)
    {
        DefinirNome(nome);
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

    public void VincularTipoDocumento(TipoDocumento tipoDocumento)
    {
        if (tipoDocumento.TenantId != TenantId)
            throw new ArgumentException("Tipo de documento deve pertencer ao mesmo tenant");

        if (_tiposDocumentoVinculados.Any(x => x.TipoDocumentoId == tipoDocumento.Id))
            return; // Já vinculado

        var vinculo = new TipoDonoTipoDocumento(Id, tipoDocumento.Id, TenantId);
        _tiposDocumentoVinculados.Add(vinculo);
    }

    public void DesvincularTipoDocumento(Guid tipoDocumentoId)
    {
        var vinculo = _tiposDocumentoVinculados.FirstOrDefault(x => x.TipoDocumentoId == tipoDocumentoId);
        if (vinculo != null)
        {
            _tiposDocumentoVinculados.Remove(vinculo);
        }
    }

    public bool PodeReceberTipoDocumento(Guid tipoDocumentoId)
    {
        return _tiposDocumentoVinculados.Any(x => x.TipoDocumentoId == tipoDocumentoId);
    }
}
