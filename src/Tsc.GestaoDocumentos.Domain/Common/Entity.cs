using DddBase.Base;

namespace Tsc.GestaoDocumentos.Domain.Common;

/// <summary>
/// Classe base para todas as entidades do domínio.
/// Herda de EntidadeBase do pacote Tsc.DddBase.
/// </summary>
public abstract class Entity : EntidadeBase<EntityId>
{
    /// <summary>
    /// Data de criação da entidade.
    /// </summary>
    public DateTime DataCriacao { get; private set; }

    /// <summary>
    /// Data da última atualização da entidade.
    /// </summary>
    public DateTime DataAtualizacao { get; private set; }

    /// <summary>
    /// Usuário que criou a entidade.
    /// </summary>
    public Guid UsuarioCriacao { get; protected set; }

    /// <summary>
    /// Usuário que fez a última alteração na entidade.
    /// </summary>
    public Guid UsuarioUltimaAlteracao { get; protected set; }

    /// <summary>
    /// Construtor protegido para uso por classes derivadas.
    /// Gera automaticamente um novo ID único.
    /// </summary>
    protected Entity()
    {
        Id = EntityId.NovoId();
        DataCriacao = DateTime.UtcNow;
        DataAtualizacao = DateTime.UtcNow;
    }

    /// <summary>
    /// Construtor protegido com ID específico para uso por classes derivadas.
    /// </summary>
    /// <param name="id">Identificador único da entidade</param>
    protected Entity(EntityId id)
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
        DataCriacao = DateTime.UtcNow;
        DataAtualizacao = DateTime.UtcNow;
    }

    /// <summary>
    /// Atualiza a data de modificação da entidade.
    /// </summary>
    protected void AtualizarDataModificacao()
    {
        DataAtualizacao = DateTime.UtcNow;
    }

    /// <summary>
    /// Atualiza a data de alteração e o usuário que fez a alteração.
    /// </summary>
    /// <param name="usuarioAlteracao">ID do usuário que fez a alteração</param>
    protected void AtualizarDataAlteracao(Guid usuarioAlteracao)
    {
        DataAtualizacao = DateTime.UtcNow;
        UsuarioUltimaAlteracao = usuarioAlteracao;
    }
}