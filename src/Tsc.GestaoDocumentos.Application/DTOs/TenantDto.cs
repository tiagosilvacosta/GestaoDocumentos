using Tsc.GestaoDocumentos.Application.DTOs.Common;

namespace Tsc.GestaoDocumentos.Application.DTOs;

public class TenantDto : BaseDto
{
    public string NomeOrganizacao { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime? DataExpiracao { get; set; }
}

public class CreateTenantDto
{
    public string NomeOrganizacao { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public DateTime? DataExpiracao { get; set; }
}

public class UpdateTenantDto
{
    public string NomeOrganizacao { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime? DataExpiracao { get; set; }
}
