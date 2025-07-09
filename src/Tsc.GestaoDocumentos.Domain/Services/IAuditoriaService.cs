using Tsc.GestaoDocumentos.Domain.Entities;
using Tsc.GestaoDocumentos.Domain.Enums;

namespace Tsc.GestaoDocumentos.Domain.Services;

public interface IAuditoriaService
{
    Task RegistrarOperacaoAsync(
        Guid tenantId,
        Guid usuarioId,
        string entidadeAfetada,
        Guid entidadeId,
        TipoOperacaoAuditoria operacao,
        string ipUsuario,
        object? dadosAnteriores = null,
        object? dadosNovos = null,
        string? userAgent = null,
        CancellationToken cancellationToken = default);

    Task RegistrarLoginAsync(
        Guid tenantId,
        Guid usuarioId,
        string ipUsuario,
        string? userAgent = null,
        CancellationToken cancellationToken = default);

    Task RegistrarLogoutAsync(
        Guid tenantId,
        Guid usuarioId,
        string ipUsuario,
        string? userAgent = null,
        CancellationToken cancellationToken = default);

    Task RegistrarDownloadDocumentoAsync(
        Guid tenantId,
        Guid usuarioId,
        Guid documentoId,
        string ipUsuario,
        string? userAgent = null,
        CancellationToken cancellationToken = default);
}
