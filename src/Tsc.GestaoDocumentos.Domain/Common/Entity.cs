namespace Tsc.GestaoDocumentos.Domain.Common;

public abstract class Entity
{
    public Guid Id { get; protected set; }
    public DateTime DataCriacao { get; protected set; }
    public Guid UsuarioCriacao { get; protected set; }
    public DateTime DataUltimaAlteracao { get; protected set; }
    public Guid UsuarioUltimaAlteracao { get; protected set; }

    protected Entity()
    {
        Id = Guid.NewGuid();
        DataCriacao = DateTime.UtcNow;
        DataUltimaAlteracao = DateTime.UtcNow;
    }

    protected Entity(Guid id) : this()
    {
        Id = id;
    }

    public void AtualizarDataAlteracao(Guid usuarioId)
    {
        DataUltimaAlteracao = DateTime.UtcNow;
        UsuarioUltimaAlteracao = usuarioId;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Entity other)
            return false;

        if (ReferenceEquals(this, other))
            return true;

        if (GetType() != other.GetType())
            return false;

        return Id == other.Id;
    }

    public static bool operator ==(Entity? a, Entity? b)
    {
        if (a is null && b is null)
            return true;

        if (a is null || b is null)
            return false;

        return a.Equals(b);
    }

    public static bool operator !=(Entity? a, Entity? b)
    {
        return !(a == b);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}
