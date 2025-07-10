using DddBase.Base;

namespace Tsc.GestaoDocumentos.Domain.Common;

/// <summary>
/// Classe base para Aggregate Roots seguindo padrões DDD
/// Inclui suporte a Domain Events e implementa IRaizAgregado
/// </summary>
public abstract class AggregateRoot : Entity, IRaizAgregado
{

    protected AggregateRoot() : base()
    {
    }

    protected AggregateRoot(EntityId id) : base(id)
    {
    }
}
