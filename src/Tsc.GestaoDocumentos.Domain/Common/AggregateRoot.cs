using DddBase.Base;

namespace Tsc.GestaoDocumentos.Domain.Common;

/// <summary>
/// Classe base para Aggregate Roots seguindo padrões DDD
/// Inclui suporte a Domain Events e implementa IRaizAgregado
/// </summary>
public abstract class AggregateRoot : Entity, IRaizAgregado
{
    private readonly List<DomainEvent> _domainEvents = new();

    /// <summary>
    /// Lista de eventos de domínio pendentes
    /// </summary>
    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected AggregateRoot() : base()
    {
    }

    protected AggregateRoot(EntityId id) : base(id)
    {
    }

    /// <summary>
    /// Adiciona um evento de domínio à lista de eventos pendentes
    /// </summary>
    /// <param name="domainEvent">Evento de domínio a ser adicionado</param>
    protected void AdicionarEventoDominio(DomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    /// <summary>
    /// Remove um evento de domínio da lista de eventos pendentes
    /// </summary>
    /// <param name="domainEvent">Evento de domínio a ser removido</param>
    protected void RemoverEventoDominio(DomainEvent domainEvent)
    {
        _domainEvents.Remove(domainEvent);
    }

    /// <summary>
    /// Limpa todos os eventos de domínio pendentes
    /// </summary>
    public void LimparEventosDominio()
    {
        _domainEvents.Clear();
    }
}
