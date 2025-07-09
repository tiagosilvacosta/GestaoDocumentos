namespace Tsc.GestaoDocumentos.Domain.Common;

public interface IDomainEvent
{
    Guid EventId { get; }
    DateTime OccurredOn { get; }
}

public abstract class DomainEvent : IDomainEvent
{
    public Guid EventId { get; private set; }
    public DateTime OccurredOn { get; private set; }

    protected DomainEvent()
    {
        EventId = Guid.NewGuid();
        OccurredOn = DateTime.UtcNow;
    }
}
