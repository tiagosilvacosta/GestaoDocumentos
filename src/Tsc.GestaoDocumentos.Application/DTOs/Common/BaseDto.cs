using Tsc.GestaoDocumentos.Domain.Organizacoes;

namespace Tsc.GestaoDocumentos.Application.DTOs.Common;

public class BaseDto
{
    public Guid Id { get; set; }
    public DateTime DataCriacao { get; set; }
    public Guid UsuarioCriacao { get; set; }
    public DateTime DataUltimaAlteracao { get; set; }
    public Guid UsuarioUltimaAlteracao { get; set; }
}

public class OrganizacaoBaseDto : BaseDto
{
    public IdOrganizacao IdOrganizacao { get; set; } = null!;
}

public class PagedResult<T>
{
    public IEnumerable<T> Items { get; set; } = [];
    public int TotalItems { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
    public bool HasNextPage => PageNumber < TotalPages;
    public bool HasPreviousPage => PageNumber > 1;
}

public class PagedRequest
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? Search { get; set; }
    public string? OrderBy { get; set; }
    public bool OrderDescending { get; set; } = false;
}
