using Tsc.GestaoDocumentos.Domain.Common;
using Tsc.GestaoDocumentos.Domain.Enums;

namespace Tsc.GestaoDocumentos.Domain.Entities;

public class Documento : TenantEntity
{
    public string NomeArquivo { get; private set; } = string.Empty;
    public string ChaveArmazenamento { get; private set; } = string.Empty;
    public DateTime DataUpload { get; private set; }
    public long TamanhoArquivo { get; private set; }
    public string TipoArquivo { get; private set; } = string.Empty;
    public int Versao { get; private set; }
    public StatusDocumento Status { get; private set; }
    public Guid TipoDocumentoId { get; private set; }

    // Navegação
    public Tenant Tenant { get; private set; } = null!;
    public TipoDocumento TipoDocumento { get; private set; } = null!;
    
    private readonly List<DocumentoDonoDocumento> _donosVinculados = new();
    public IReadOnlyCollection<DocumentoDonoDocumento> DonosVinculados => _donosVinculados.AsReadOnly();

    protected Documento() : base() { }

    public Documento(
        Guid tenantId,
        string nomeArquivo,
        string chaveArmazenamento,
        long tamanhoArquivo,
        string tipoArquivo,
        Guid tipoDocumentoId,
        Guid usuarioCriacao)
        : base(tenantId)
    {
        DefinirNomeArquivo(nomeArquivo);
        DefinirChaveArmazenamento(chaveArmazenamento);
        DefinirTamanhoArquivo(tamanhoArquivo);
        DefinirTipoArquivo(tipoArquivo);
        TipoDocumentoId = tipoDocumentoId;
        DataUpload = DateTime.UtcNow;
        Versao = 1;
        Status = StatusDocumento.Ativo;
        UsuarioCriacao = usuarioCriacao;
        UsuarioUltimaAlteracao = usuarioCriacao;
    }

    public void DefinirNomeArquivo(string nomeArquivo)
    {
        if (string.IsNullOrWhiteSpace(nomeArquivo))
            throw new ArgumentException("Nome do arquivo é obrigatório", nameof(nomeArquivo));

        if (nomeArquivo.Length > 255)
            throw new ArgumentException("Nome do arquivo não pode ter mais de 255 caracteres", nameof(nomeArquivo));

        NomeArquivo = nomeArquivo.Trim();
    }

    public void DefinirChaveArmazenamento(string chaveArmazenamento)
    {
        if (string.IsNullOrWhiteSpace(chaveArmazenamento))
            throw new ArgumentException("Chave de armazenamento é obrigatória", nameof(chaveArmazenamento));

        if (chaveArmazenamento.Length > 500)
            throw new ArgumentException("Chave de armazenamento não pode ter mais de 500 caracteres", nameof(chaveArmazenamento));

        ChaveArmazenamento = chaveArmazenamento.Trim();
    }

    public void DefinirTamanhoArquivo(long tamanhoArquivo)
    {
        if (tamanhoArquivo <= 0)
            throw new ArgumentException("Tamanho do arquivo deve ser maior que zero", nameof(tamanhoArquivo));

        TamanhoArquivo = tamanhoArquivo;
    }

    public void DefinirTipoArquivo(string tipoArquivo)
    {
        if (string.IsNullOrWhiteSpace(tipoArquivo))
            throw new ArgumentException("Tipo do arquivo é obrigatório", nameof(tipoArquivo));

        if (tipoArquivo.Length > 50)
            throw new ArgumentException("Tipo do arquivo não pode ter mais de 50 caracteres", nameof(tipoArquivo));

        TipoArquivo = tipoArquivo.Trim().ToLowerInvariant();
    }

    public void AlterarStatus(StatusDocumento novoStatus, Guid usuarioAlteracao)
    {
        Status = novoStatus;
        AtualizarDataAlteracao(usuarioAlteracao);
    }

    public void DefinirVersao(int versao)
    {
        if (versao <= 0)
            throw new ArgumentException("Versão deve ser maior que zero", nameof(versao));

        Versao = versao;
    }

    public void VincularDonoDocumento(DonoDocumento donoDocumento)
    {
        if (donoDocumento.TenantId != TenantId)
            throw new ArgumentException("Dono do documento deve pertencer ao mesmo tenant");

        if (_donosVinculados.Any(x => x.DonoDocumentoId == donoDocumento.Id.Valor))
            return; // Já vinculado

        var vinculo = new DocumentoDonoDocumento(Id.Valor, donoDocumento.Id.Valor, TenantId);
        _donosVinculados.Add(vinculo);
    }

    public void DesvincularDonoDocumento(Guid donoDocumentoId)
    {
        var vinculo = _donosVinculados.FirstOrDefault(x => x.DonoDocumentoId == donoDocumentoId);
        if (vinculo != null)
        {
            _donosVinculados.Remove(vinculo);
        }
    }

    public bool EstaAtivo() => Status == StatusDocumento.Ativo;

    public string ObterExtensao()
    {
        return Path.GetExtension(NomeArquivo).ToLowerInvariant();
    }

    public string ObterTamanhoFormatado()
    {
        string[] tamanhos = { "B", "KB", "MB", "GB", "TB" };
        double tamanho = TamanhoArquivo;
        int ordem = 0;
        
        while (tamanho >= 1024 && ordem < tamanhos.Length - 1)
        {
            ordem++;
            tamanho = tamanho / 1024;
        }

        return $"{tamanho:0.##} {tamanhos[ordem]}";
    }
}
