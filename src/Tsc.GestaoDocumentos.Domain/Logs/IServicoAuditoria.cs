using Tsc.GestaoDocumentos.Domain.Documentos;
using Tsc.GestaoDocumentos.Domain.Organizacoes;
using Tsc.GestaoDocumentos.Domain.Usuarios;

namespace Tsc.GestaoDocumentos.Domain.Logs;

public interface IServicoAuditoria
{
    Task RegistrarOperacaoAsync(
        IdOrganizacao idOrganizacao,
        IdUsuario idUsuario,
        string entidadeAfetada,
        Guid entidadeId,
        TipoOperacaoAuditoria operacao,
        string ipUsuario,
        object? dadosAnteriores = null,
        object? dadosNovos = null,
        string? userAgent = null,
        CancellationToken cancellationToken = default);

    Task RegistrarLoginAsync(
        IdOrganizacao idOrganizacao,
        IdUsuario idUsuario,
        string ipUsuario,
        string? userAgent = null,
        CancellationToken cancellationToken = default);

    Task RegistrarLogoutAsync(
        IdOrganizacao idOrganizacao,
        IdUsuario idUsuario,
        string ipUsuario,
        string? userAgent = null,
        CancellationToken cancellationToken = default);

    Task RegistrarDownloadDocumentoAsync(
        IdOrganizacao idOrganizacao,
        IdUsuario idUsuario,
        IdDocumento documentoId,
        string ipUsuario,
        string? userAgent = null,
        CancellationToken cancellationToken = default);
}
