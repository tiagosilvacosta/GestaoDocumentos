using System.Text.Json;
using Tsc.GestaoDocumentos.Domain.Common;
using Tsc.GestaoDocumentos.Domain.Entities;
using Tsc.GestaoDocumentos.Domain.Enums;
using Tsc.GestaoDocumentos.Domain.Services;

namespace Tsc.GestaoDocumentos.Infrastructure.Services;

public class AuditoriaService : IAuditoriaService
{
    private readonly IUnitOfWork _unitOfWork;

    public AuditoriaService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task RegistrarOperacaoAsync(
        Guid tenantId,
        Guid usuarioId,
        string entidadeAfetada,
        Guid entidadeId,
        TipoOperacaoAuditoria operacao,
        string ipUsuario,
        object? dadosAnteriores = null,
        object? dadosNovos = null,
        string? userAgent = null,
        CancellationToken cancellationToken = default)
    {
        var logAuditoria = new LogAuditoria(
            tenantId,
            usuarioId,
            entidadeAfetada,
            entidadeId,
            operacao,
            ipUsuario,
            dadosAnteriores != null ? JsonSerializer.Serialize(dadosAnteriores) : null,
            dadosNovos != null ? JsonSerializer.Serialize(dadosNovos) : null,
            userAgent);

        await _unitOfWork.LogsAuditoria.AdicionarAsync(logAuditoria, cancellationToken);
    }

    public async Task RegistrarLoginAsync(
        Guid tenantId,
        Guid usuarioId,
        string ipUsuario,
        string? userAgent = null,
        CancellationToken cancellationToken = default)
    {
        await RegistrarOperacaoAsync(
            tenantId,
            usuarioId,
            nameof(Usuario),
            usuarioId,
            TipoOperacaoAuditoria.LOGIN,
            ipUsuario,
            null,
            new { Acao = "Login realizado" },
            userAgent,
            cancellationToken);
    }

    public async Task RegistrarLogoutAsync(
        Guid tenantId,
        Guid usuarioId,
        string ipUsuario,
        string? userAgent = null,
        CancellationToken cancellationToken = default)
    {
        await RegistrarOperacaoAsync(
            tenantId,
            usuarioId,
            nameof(Usuario),
            usuarioId,
            TipoOperacaoAuditoria.LOGOUT,
            ipUsuario,
            null,
            new { Acao = "Logout realizado" },
            userAgent,
            cancellationToken);
    }

    public async Task RegistrarDownloadDocumentoAsync(
        Guid tenantId,
        Guid usuarioId,
        Guid documentoId,
        string ipUsuario,
        string? userAgent = null,
        CancellationToken cancellationToken = default)
    {
        await RegistrarOperacaoAsync(
            tenantId,
            usuarioId,
            nameof(Documento),
            documentoId,
            TipoOperacaoAuditoria.DOWNLOAD,
            ipUsuario,
            null,
            new { Acao = "Download de documento" },
            userAgent,
            cancellationToken);
    }
}
