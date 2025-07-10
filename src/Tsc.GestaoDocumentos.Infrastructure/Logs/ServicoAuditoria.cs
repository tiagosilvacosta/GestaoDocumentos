using System.Text.Json;
using Tsc.GestaoDocumentos.Domain.Common;
using Tsc.GestaoDocumentos.Domain.Documentos;
using Tsc.GestaoDocumentos.Domain.Logs;
using Tsc.GestaoDocumentos.Domain.Organizacoes;
using Tsc.GestaoDocumentos.Domain.Usuarios;

namespace Tsc.GestaoDocumentos.Infrastructure.Logs;

public class ServicoAuditoria : IServicoAuditoria
{
    private readonly IUnitOfWork _unitOfWork;

    public ServicoAuditoria(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task RegistrarOperacaoAsync(
        IdOrganizacao idOrganizacao,
        IdUsuario idUsuario,
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
            idOrganizacao,
            idUsuario,
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
        IdOrganizacao idOrganizacao,
        IdUsuario idUsuario,
        string ipUsuario,
        string? userAgent = null,
        CancellationToken cancellationToken = default)
    {
        await RegistrarOperacaoAsync(
            idOrganizacao,
            idUsuario,
            nameof(Usuario),
            idUsuario.Valor,
            TipoOperacaoAuditoria.LOGIN,
            ipUsuario,
            null,
            new { Acao = "Login realizado" },
            userAgent,
            cancellationToken);
    }

    public async Task RegistrarLogoutAsync(
        IdOrganizacao idOrganizacao,
        IdUsuario idUsuario,
        string ipUsuario,
        string? userAgent = null,
        CancellationToken cancellationToken = default)
    {
        await RegistrarOperacaoAsync(
            idOrganizacao,
            idUsuario,
            nameof(Usuario),
            idUsuario.Valor,
            TipoOperacaoAuditoria.LOGOUT,
            ipUsuario,
            null,
            new { Acao = "Logout realizado" },
            userAgent,
            cancellationToken);
    }

    public async Task RegistrarDownloadDocumentoAsync(
        IdOrganizacao idOrganizacao,
        IdUsuario idUsuario,
        IdDocumento IdDocumento,
        string ipUsuario,
        string? userAgent = null,
        CancellationToken cancellationToken = default)
    {
        await RegistrarOperacaoAsync(
            idOrganizacao,
            idUsuario,
            nameof(Documento),
            IdDocumento.Valor,
            TipoOperacaoAuditoria.DOWNLOAD,
            ipUsuario,
            null,
            new { Acao = "Download de documento" },
            userAgent,
            cancellationToken);
    }
}
