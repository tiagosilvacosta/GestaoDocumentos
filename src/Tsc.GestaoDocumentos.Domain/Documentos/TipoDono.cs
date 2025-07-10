using DddBase.Base;
using Tsc.GestaoDocumentos.Domain.Common;
using Tsc.GestaoDocumentos.Domain.Organizacoes;
using Tsc.GestaoDocumentos.Domain.Usuarios;

namespace Tsc.GestaoDocumentos.Domain.Documentos;

public class TipoDono : EntidadeComAuditoriaEOrganizacao<IdTipoDono>, IRaizAgregado
{
    public string Nome { get; private set; } = string.Empty;

    // Navegação
    public Organizacao Tenant { get; private set; } = null!;
    
    private readonly List<DonoDocumento> _donosDocumento = new();
    public IReadOnlyCollection<DonoDocumento> DonosDocumento => _donosDocumento.AsReadOnly();

    private readonly List<TipoDonoTipoDocumento> _tiposDocumentoVinculados = new();
    public IReadOnlyCollection<TipoDonoTipoDocumento> TiposDocumentoVinculados => _tiposDocumentoVinculados.AsReadOnly();

    protected TipoDono() : base() { }

    public TipoDono(IdOrganizacao idOrganizacao, string nome, IdUsuario usuarioCriacao)
        : base(IdTipoDono.CriarNovo(), idOrganizacao)
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
        if (tipoDocumento.IdOrganizacao != IdOrganizacao)
            throw new ArgumentException("Tipo de documento deve pertencer ao mesmo tenant");

        if (_tiposDocumentoVinculados.Any(x => x.IdTipoDocumento == tipoDocumento.Id))
            return; // Já vinculado

        var vinculo = new TipoDonoTipoDocumento(Id, tipoDocumento.Id, IdOrganizacao);
        _tiposDocumentoVinculados.Add(vinculo);
    }

    public void DesvincularTipoDocumento(IdTipoDocumento idTipoDocumento)
    {
        var vinculo = _tiposDocumentoVinculados.FirstOrDefault(x => x.IdTipoDocumento == idTipoDocumento);
        if (vinculo != null)
        {
            _tiposDocumentoVinculados.Remove(vinculo);
        }
    }

    public bool PodeReceberTipoDocumento(IdTipoDocumento idTipoDocumento)
    {
        return _tiposDocumentoVinculados.Any(x => x.IdTipoDocumento == idTipoDocumento);
    }
}
