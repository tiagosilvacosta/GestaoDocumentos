using DddBase.Base;
using Tsc.GestaoDocumentos.Domain.Organizacoes;
using Tsc.GestaoDocumentos.Domain.Usuarios;

namespace Tsc.GestaoDocumentos.Domain.Documentos;

public class TipoDocumento : EntidadeComAuditoriaEOrganizacao<IdTipoDocumento>, IRaizAgregado
{
    public string Nome { get; private set; } = string.Empty;
    public bool PermiteMultiplosDocumentos { get; private set; }

    // Navegação
    public Organizacao Tenant { get; private set; } = null!;
    
    private readonly List<Documento> _documentos = new();
    public IReadOnlyCollection<Documento> Documentos => _documentos.AsReadOnly();

    private readonly List<TipoDonoTipoDocumento> _tiposDonoVinculados = new();
    public IReadOnlyCollection<TipoDonoTipoDocumento> TiposDonoVinculados => _tiposDonoVinculados.AsReadOnly();

    protected TipoDocumento() : base() { }

    public TipoDocumento(
        IdOrganizacao idOrganizacao, 
        string nome, 
        bool permiteMultiplosDocumentos, 
        IdUsuario usuarioCriacao)
        : base(IdTipoDocumento.CriarNovo(), idOrganizacao)
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

    public void AlterarPermissaoMultiplosDocumentos(bool permiteMultiplos, IdUsuario usuarioAlteracao)
    {
        PermiteMultiplosDocumentos = permiteMultiplos;
        AtualizarDataAlteracao(usuarioAlteracao);
    }

    public void VincularTipoDono(TipoDono tipoDono)
    {
        if (tipoDono.IdOrganizacao != IdOrganizacao)
            throw new ArgumentException("Tipo de dono deve pertencer ao mesmo tenant");

        if (_tiposDonoVinculados.Any(x => x.IdTipoDono == tipoDono.Id))
            return; // Já vinculado

        var vinculo = new TipoDonoTipoDocumento(tipoDono.Id, Id, IdOrganizacao);
        _tiposDonoVinculados.Add(vinculo);
    }

    public void DesvincularTipoDono(IdTipoDono idTipoDono)
    {
        var vinculo = _tiposDonoVinculados.FirstOrDefault(x => x.IdTipoDono == idTipoDono);
        if (vinculo != null)
        {
            _tiposDonoVinculados.Remove(vinculo);
        }
    }

    public bool PodeSerUsadoPorTipoDono(IdTipoDono idTipoDono)
    {
        return _tiposDonoVinculados.Any(x => x.IdTipoDono == idTipoDono);
    }
}
