using Tsc.GestaoDocumentos.Domain.Common;

namespace Tsc.GestaoDocumentos.Domain.Entities;

public class TipoDocumento : TenantEntity
{
    public string Nome { get; private set; } = string.Empty;
    public bool PermiteMultiplosDocumentos { get; private set; }

    // Navegação
    public Tenant Tenant { get; private set; } = null!;
    
    private readonly List<Documento> _documentos = new();
    public IReadOnlyCollection<Documento> Documentos => _documentos.AsReadOnly();

    private readonly List<TipoDonoTipoDocumento> _tiposDonoVinculados = new();
    public IReadOnlyCollection<TipoDonoTipoDocumento> TiposDonoVinculados => _tiposDonoVinculados.AsReadOnly();

    protected TipoDocumento() : base() { }

    public TipoDocumento(
        Guid tenantId, 
        string nome, 
        bool permiteMultiplosDocumentos, 
        Guid usuarioCriacao)
        : base(tenantId)
    {
        DefinirNome(nome);
        PermiteMultiplosDocumentos = permiteMultiplosDocumentos;
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

    public void AlterarPermissaoMultiplosDocumentos(bool permiteMultiplos, Guid usuarioAlteracao)
    {
        PermiteMultiplosDocumentos = permiteMultiplos;
        AtualizarDataAlteracao(usuarioAlteracao);
    }

    public void VincularTipoDono(TipoDono tipoDono)
    {
        if (tipoDono.TenantId != TenantId)
            throw new ArgumentException("Tipo de dono deve pertencer ao mesmo tenant");

        if (_tiposDonoVinculados.Any(x => x.TipoDonoId == tipoDono.Id.Valor))
            return; // Já vinculado

        var vinculo = new TipoDonoTipoDocumento(tipoDono.Id.Valor, Id.Valor, TenantId);
        _tiposDonoVinculados.Add(vinculo);
    }

    public void DesvincularTipoDono(Guid tipoDonoId)
    {
        var vinculo = _tiposDonoVinculados.FirstOrDefault(x => x.TipoDonoId == tipoDonoId);
        if (vinculo != null)
        {
            _tiposDonoVinculados.Remove(vinculo);
        }
    }

    public bool PodeSerUsadoPorTipoDono(Guid tipoDonoId)
    {
        return _tiposDonoVinculados.Any(x => x.TipoDonoId == tipoDonoId);
    }
}
