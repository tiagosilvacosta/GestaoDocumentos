namespace Tsc.GestaoDocumentos.Domain.Services;

public interface IArmazenamentoArquivoService
{
    Task<string> ArmazenarArquivoAsync(Stream arquivo, string nomeArquivo, IdOrganizacao idOrganizacao, CancellationToken cancellationToken = default);
    Task<Stream> ObterArquivoAsync(string chaveArmazenamento, CancellationToken cancellationToken = default);
    Task ExcluirArquivoAsync(string chaveArmazenamento, CancellationToken cancellationToken = default);
    Task<bool> ArquivoExisteAsync(string chaveArmazenamento, CancellationToken cancellationToken = default);
    string GerarChaveArmazenamento(string nomeArquivo, IdOrganizacao idOrganizacao, Guid documentoId);
}
