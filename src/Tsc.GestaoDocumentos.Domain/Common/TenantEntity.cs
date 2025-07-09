namespace Tsc.GestaoDocumentos.Domain.Common;

public abstract class TenantEntity : Entity
{
    public Guid TenantId { get; protected set; }

    protected TenantEntity() : base()
    {
    }

    protected TenantEntity(Guid tenantId) : base()
    {
        TenantId = tenantId;
    }

    protected TenantEntity(Guid id, Guid tenantId) : base(id)
    {
        TenantId = tenantId;
    }

    public void DefinirTenant(Guid tenantId)
    {
        if (tenantId == Guid.Empty)
            throw new ArgumentException("TenantId não pode ser vazio", nameof(tenantId));

        TenantId = tenantId;
    }
}
