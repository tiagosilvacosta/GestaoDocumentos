using DddBase.Base;
using Tsc.GestaoDocumentos.Domain.Organizacoes;
using Tsc.GestaoDocumentos.Domain.Usuarios;

namespace Tsc.GestaoDocumentos.Domain.Documentos;

public enum StatusDocumento
{
    Ativo = 1,
    Inativo = 2
}

public class Documento : EntidadeComAuditoriaEOrganizacao<IdDocumento>, IRaizAgregado
{
    public string NomeArquivo { get; private set; } = string.Empty;
    public string ChaveArmazenamento { get; private set; } = string.Empty;
    public DateTime DataUpload { get; private set; }
    public long TamanhoArquivo { get; private set; }
    public string TipoArquivo { get; private set; } = string.Empty;
    public int Versao { get; private set; }
    public StatusDocumento Status { get; private set; }
    public IdTipoDocumento IdTipoDocumento { get; private set; } = null!;

    // Navegação
    public Organizacao Organizacao { get; private set; } = null!;
    public TipoDocumento TipoDocumento { get; private set; } = null!;
    
    private readonly List<DocumentoDonoDocumento> _donosVinculados = [];
    public IReadOnlyCollection<DocumentoDonoDocumento> DonosVinculados => _donosVinculados.AsReadOnly();

    protected Documento() : base() { }

    public Documento(
        IdOrganizacao idOrganizacao,
        string nomeArquivo,
        string chaveArmazenamento,
        long tamanhoArquivo,
        string tipoArquivo,
        IdTipoDocumento idTipoDocumento,
        IdUsuario usuarioCriacao)
        : base(IdDocumento.CriarNovo(), idOrganizacao)
    {
        DefinirNomeArquivo(nomeArquivo);
        DefinirChaveArmazenamento(chaveArmazenamento);
        DefinirTamanhoArquivo(tamanhoArquivo);
        DefinirTipoArquivo(tipoArquivo);
        IdTipoDocumento = idTipoDocumento;
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

    public void AlterarStatus(StatusDocumento novoStatus, IdUsuario usuarioAlteracao)
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
        if (donoDocumento.IdOrganizacao != IdOrganizacao)
            throw new ArgumentException("Dono do documento deve pertencer ao mesmo tenant");

        if (_donosVinculados.Any(x => x.IdDonoDocumento == donoDocumento.Id))
            return; // Já vinculado

        var vinculo = new DocumentoDonoDocumento(this, donoDocumento, IdOrganizacao);
        _donosVinculados.Add(vinculo);
    }

    public void DesvincularDonoDocumento(IdDonoDocumento idDonoDocumento)
    {
        var vinculo = _donosVinculados.FirstOrDefault(x => x.IdDocumento == idDonoDocumento);
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
        string[] tamanhos = ["B", "KB", "MB", "GB", "TB"];
        double tamanho = TamanhoArquivo;
        int ordem = 0;
        
        while (tamanho >= 1024 && ordem < tamanhos.Length - 1)
        {
            ordem++;
            tamanho /= 1024;
        }

        return $"{tamanho:0.##} {tamanhos[ordem]}";
    }
}
