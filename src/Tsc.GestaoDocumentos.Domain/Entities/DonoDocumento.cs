using Tsc.GestaoDocumentos.Domain.Common;

namespace Tsc.GestaoDocumentos.Domain.Entities;

public class DonoDocumento : TenantEntity
{
    public string NomeAmigavel { get; private set; } = string.Empty;
    public Guid TipoDonoId { get; private set; }

    // Navegação
    public Tenant Tenant { get; private set; } = null!;
    public TipoDono TipoDono { get; private set; } = null!;
    
    private readonly List<DocumentoDonoDocumento> _documentosVinculados = new();
    public IReadOnlyCollection<DocumentoDonoDocumento> DocumentosVinculados => _documentosVinculados.AsReadOnly();

    protected DonoDocumento() : base() { }

    public DonoDocumento(
        Guid tenantId,
        string nomeAmigavel,
        Guid tipoDonoId,
        Guid usuarioCriacao)
        : base(tenantId)
    {
        DefinirNomeAmigavel(nomeAmigavel);
        TipoDonoId = tipoDonoId;
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
        if (documento.TenantId != TenantId)
            throw new ArgumentException("Documento deve pertencer ao mesmo tenant");

        // Verificar se o tipo de documento é compatível com o tipo de dono
        if (!TipoDono.PodeReceberTipoDocumento(documento.TipoDocumentoId))
            throw new InvalidOperationException("Tipo de documento não é compatível com o tipo de dono");

        // Verificar se já existe documento ativo do mesmo tipo se não permite múltiplos
        if (!documento.TipoDocumento.PermiteMultiplosDocumentos)
        {
            var documentoExistente = _documentosVinculados
                .Where(x => x.Documento.TipoDocumentoId == documento.TipoDocumentoId)
                .Any(x => x.Documento.EstaAtivo());

            if (documentoExistente)
                throw new InvalidOperationException("Já existe um documento ativo deste tipo para este dono");
        }

        if (_documentosVinculados.Any(x => x.DocumentoId == documento.Id))
            return; // Já vinculado

        var vinculo = new DocumentoDonoDocumento(documento.Id, Id, TenantId);
        _documentosVinculados.Add(vinculo);
    }

    public void DesvincularDocumento(Guid documentoId)
    {
        var vinculo = _documentosVinculados.FirstOrDefault(x => x.DocumentoId == documentoId);
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

    public IEnumerable<Documento> ObterDocumentosPorTipo(Guid tipoDocumentoId)
    {
        return _documentosVinculados
            .Where(x => x.Documento.TipoDocumentoId == tipoDocumentoId)
            .Select(x => x.Documento);
    }
}
