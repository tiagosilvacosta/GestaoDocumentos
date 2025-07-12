using DddBase.Base;
using Tsc.GestaoDocumentos.Domain.Organizacoes;
using Tsc.GestaoDocumentos.Domain.Usuarios;

namespace Tsc.GestaoDocumentos.Domain.Documentos;

public class DonoDocumento : EntidadeComAuditoriaEOrganizacao<IdDonoDocumento>, IRaizAgregado
{
    public string NomeAmigavel { get; private set; } = string.Empty;
    public IdTipoDono IdTipoDono { get; private set; } = null!;

    // Navegação
    public Organizacao Organizacao { get; private set; } = null!;
    public TipoDono TipoDono { get; private set; } = null!;
    
    private readonly List<DocumentoDonoDocumento> _documentosVinculados = new();
    public IReadOnlyCollection<DocumentoDonoDocumento> DocumentosVinculados => _documentosVinculados.AsReadOnly();

    protected DonoDocumento() : base() { }

    public DonoDocumento(
        IdOrganizacao idOrganizacao,
        string nomeAmigavel,
        IdTipoDono idTipoDono,
        IdUsuario usuarioCriacao)
        : base(IdDonoDocumento.CriarNovo(), idOrganizacao)
    {
        DefinirNomeAmigavel(nomeAmigavel);
        IdTipoDono = idTipoDono;
        UsuarioCriacao = usuarioCriacao;
        UsuarioUltimaAlteracao = usuarioCriacao;
    }

    public void DefinirNomeAmigavel(string nomeAmigavel)
    {
        if (string.IsNullOrWhiteSpace(nomeAmigavel))
            throw new ArgumentException("Nome amigável é obrigatório", nameof(nomeAmigavel));

        if (nomeAmigavel.Length > 255)
            throw new ArgumentException("Nome amigável não pode ter mais de 255 caracteres", nameof(nomeAmigavel));

        NomeAmigavel = nomeAmigavel.Trim();
    }

    public void VincularDocumento(Documento documento)
    {
        if (documento.IdOrganizacao != IdOrganizacao)
            throw new ArgumentException("Documento deve pertencer ao mesmo tenant");

        // Verificar se o tipo de documento é compatível com o tipo de dono
        if (!TipoDono.PodeReceberTipoDocumento(documento.IdTipoDocumento))
            throw new InvalidOperationException("Tipo de documento não é compatível com o tipo de dono");

        // Verificar se já existe documento ativo do mesmo tipo se não permite múltiplos
        if (!documento.TipoDocumento.PermiteMultiplosDocumentos)
        {
            var documentoExistente = _documentosVinculados
                .Where(x => x.Documento.IdTipoDocumento == documento.IdTipoDocumento)
                .Any(x => x.Documento.EstaAtivo());

            if (documentoExistente)
                throw new InvalidOperationException("Já existe um documento ativo deste tipo para este dono");
        }

        if (_documentosVinculados.Any(x => x.IdDocumento == documento.Id))
            return; // Já vinculado

        var vinculo = new DocumentoDonoDocumento(documento, this, IdOrganizacao);
        _documentosVinculados.Add(vinculo);
    }

    public void DesvincularDocumento(IdDocumento idDocumento)
    {
        var vinculo = _documentosVinculados.FirstOrDefault(x => x.IdDocumento == idDocumento);
        if (vinculo != null)
        {
            _documentosVinculados.Remove(vinculo);
        }
    }

    public IEnumerable<Documento> ObterDocumentosAtivos()
    {
        return _documentosVinculados
            .Where(x => x.Documento.EstaAtivo())
            .Select(x => x.Documento);
    }

    public IEnumerable<Documento> ObterDocumentosPorTipo(IdTipoDocumento idTipoDocumento)
    {
        return _documentosVinculados
            .Where(x => x.Documento.IdTipoDocumento == idTipoDocumento)
            .Select(x => x.Documento);
    }
}
