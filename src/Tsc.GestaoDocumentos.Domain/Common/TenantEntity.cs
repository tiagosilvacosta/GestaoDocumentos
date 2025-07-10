namespace Tsc.GestaoDocumentos.Domain.Common;

/// <summary>
/// Classe base para entidades que pertencem a um tenant específico.
/// Herda de AggregateRoot pois entidades tenant são tipicamente aggregate roots.
/// </summary>
public abstract class TenantEntity : AggregateRoot
{
    /// <summary>
    /// Identificador do tenant ao qual a entidade pertence.
    /// </summary>
    public Guid TenantId { get; protected set; }

    /// <summary>
    /// Construtor protegido padrão.
    /// </summary>
    protected TenantEntity() : base()
    {
    }

    /// <summary>
    /// Construtor protegido com ID do tenant.
    /// </summary>
    /// <param name="tenantId">Identificador do tenant</param>
    protected TenantEntity(Guid tenantId) : base()
    {
        DefinirTenant(tenantId);
    }

    /// <summary>
    /// Construtor protegido com ID da entidade e ID do tenant.
    /// </summary>
    /// <param name="id">Identificador da entidade</param>
    /// <param name="tenantId">Identificador do tenant</param>
    protected TenantEntity(EntityId id, Guid tenantId) : base(id)
    {
        DefinirTenant(tenantId);
    }

    /// <summary>
    /// Define o tenant da entidade.
    /// </summary>
    /// <param name="tenantId">Identificador do tenant</param>
    /// <exception cref="ArgumentException">Quando o tenantId é vazio</exception>
    public void DefinirTenant(Guid tenantId)
    {
        if (tenantId == Guid.Empty)
            throw new ArgumentException("TenantId não pode ser vazio", nameof(tenantId));

        TenantId = tenantId;
    }
}
