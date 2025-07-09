using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Tsc.GestaoDocumentos.Domain.Services;

namespace Tsc.GestaoDocumentos.Infrastructure.Services;

public class ArmazenamentoArquivoService : IArmazenamentoArquivoService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<ArmazenamentoArquivoService> _logger;
    private readonly string _basePath;

    public ArmazenamentoArquivoService(
        IConfiguration configuration,
        ILogger<ArmazenamentoArquivoService> logger)
    {
        _configuration = configuration;
        _logger = logger;
        _basePath = _configuration.GetValue<string>("ArmazenamentoArquivos:CaminhoBase") 
                   ?? Path.Combine(Directory.GetCurrentDirectory(), "uploads");
        
        // Garantir que o diretório base existe
        if (!Directory.Exists(_basePath))
        {
            Directory.CreateDirectory(_basePath);
        }
    }

    public async Task<string> ArmazenarArquivoAsync(Stream arquivo, string nomeArquivo, Guid tenantId, CancellationToken cancellationToken = default)
    {
        try
        {
            var documentoId = Guid.NewGuid();
            var chaveArmazenamento = GerarChaveArmazenamento(nomeArquivo, tenantId, documentoId);
            var caminhoCompleto = Path.Combine(_basePath, chaveArmazenamento);

            // Garantir que o diretório existe
            var diretorio = Path.GetDirectoryName(caminhoCompleto);
            if (!string.IsNullOrEmpty(diretorio) && !Directory.Exists(diretorio))
            {
                Directory.CreateDirectory(diretorio);
            }

            using var fileStream = new FileStream(caminhoCompleto, FileMode.Create, FileAccess.Write);
            await arquivo.CopyToAsync(fileStream, cancellationToken);

            _logger.LogInformation("Arquivo armazenado com sucesso: {ChaveArmazenamento}", chaveArmazenamento);
            
            return chaveArmazenamento;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao armazenar arquivo {NomeArquivo} para tenant {TenantId}", nomeArquivo, tenantId);
            throw;
        }
    }

    public async Task<Stream> ObterArquivoAsync(string chaveArmazenamento, CancellationToken cancellationToken = default)
    {
        try
        {
            var caminhoCompleto = Path.Combine(_basePath, chaveArmazenamento);
            
            if (!File.Exists(caminhoCompleto))
            {
                throw new FileNotFoundException($"Arquivo não encontrado: {chaveArmazenamento}");
            }

            var memoryStream = new MemoryStream();
            using var fileStream = new FileStream(caminhoCompleto, FileMode.Open, FileAccess.Read);
            await fileStream.CopyToAsync(memoryStream, cancellationToken);
            
            memoryStream.Position = 0;
            return memoryStream;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter arquivo {ChaveArmazenamento}", chaveArmazenamento);
            throw;
        }
    }

    public async Task ExcluirArquivoAsync(string chaveArmazenamento, CancellationToken cancellationToken = default)
    {
        try
        {
            var caminhoCompleto = Path.Combine(_basePath, chaveArmazenamento);
            
            if (File.Exists(caminhoCompleto))
            {
                File.Delete(caminhoCompleto);
                _logger.LogInformation("Arquivo excluído com sucesso: {ChaveArmazenamento}", chaveArmazenamento);
            }
            
            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir arquivo {ChaveArmazenamento}", chaveArmazenamento);
            throw;
        }
    }

    public async Task<bool> ArquivoExisteAsync(string chaveArmazenamento, CancellationToken cancellationToken = default)
    {
        try
        {
            var caminhoCompleto = Path.Combine(_basePath, chaveArmazenamento);
            return await Task.FromResult(File.Exists(caminhoCompleto));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao verificar existência do arquivo {ChaveArmazenamento}", chaveArmazenamento);
            return false;
        }
    }

    public string GerarChaveArmazenamento(string nomeArquivo, Guid tenantId, Guid documentoId)
    {
        var extensao = Path.GetExtension(nomeArquivo);
        var nomeSeguro = Path.GetFileNameWithoutExtension(nomeArquivo)
            .Replace(" ", "_")
            .Replace("\\", "")
            .Replace("/", "")
            .Replace(":", "")
            .Replace("*", "")
            .Replace("?", "")
            .Replace("\"", "")
            .Replace("<", "")
            .Replace(">", "")
            .Replace("|", "");

        var dataAtual = DateTime.UtcNow;
        var ano = dataAtual.Year;
        var mes = dataAtual.Month.ToString("D2");
        var dia = dataAtual.Day.ToString("D2");

        return Path.Combine(
            tenantId.ToString(),
            ano.ToString(),
            mes,
            dia,
            $"{documentoId}_{nomeSeguro}{extensao}");
    }
}
