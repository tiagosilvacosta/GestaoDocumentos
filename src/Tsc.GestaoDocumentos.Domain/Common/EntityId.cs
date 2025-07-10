using DddBase.Base;

namespace Tsc.GestaoDocumentos.Domain.Common;

/// <summary>
/// Objeto de valor que representa um identificador único de entidade.
/// </summary>
public record EntityId : ObjetoDeValor
{
    /// <summary>
    /// Valor do identificador.
    /// </summary>
    public Guid Valor { get; init; }

    /// <summary>
    /// Construtor privado para deserialização.
    /// </summary>
    private EntityId() 
    {
        Valor = Guid.Empty;
    }

    /// <summary>
    /// Construtor que inicializa o identificador com um valor específico.
    /// </summary>
    /// <param name="valor">Valor do identificador</param>
    public EntityId(Guid valor)
    {
        if (valor == Guid.Empty)
            throw new ArgumentException("O identificador não pode ser vazio.", nameof(valor));
            
        Valor = valor;
    }

    /// <summary>
    /// Cria um novo identificador único.
    /// </summary>
    /// <returns>Nova instância de EntityId com ID único</returns>
    public static EntityId NovoId() => new(Guid.NewGuid());

    /// <summary>
    /// Cria um identificador a partir de um Guid.
    /// </summary>
    /// <param name="guid">Valor do Guid</param>
    /// <returns>Nova instância de EntityId</returns>
    public static EntityId De(Guid guid) => new(guid);

    /// <summary>
    /// Conversão implícita de EntityId para Guid.
    /// </summary>
    /// <param name="entityId">Identificador da entidade</param>
    public static implicit operator Guid(EntityId entityId) => entityId.Valor;

    /// <summary>
    /// Conversão implícita de Guid para EntityId.
    /// </summary>
    /// <param name="guid">Valor do Guid</param>
    public static implicit operator EntityId(Guid guid) => new(guid);

    /// <summary>
    /// Representação em string do identificador.
    /// </summary>
    /// <returns>String representando o valor do ID</returns>
    public override string ToString() => Valor.ToString();
}
