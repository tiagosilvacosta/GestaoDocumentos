using DddBase.Base;
using Tsc.GestaoDocumentos.Domain.Usuarios;

namespace Tsc.GestaoDocumentos.Domain
{
    /// <summary>
    /// Classe base para todas as entidades com auditoria do domínio.
    /// Herda de EntidadeBase do pacote Tsc.DddBase.
    /// </summary>
    public class EntidadeComAuditoria<TId> : EntidadeBase<TId> where TId : ObjetoDeValor
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
        public IdUsuario UsuarioCriacao { get; protected set; }

        /// <summary>
        /// Usuário que fez a última alteração na entidade.
        /// </summary>
        public IdUsuario UsuarioUltimaAlteracao { get; protected set; }

        /// <summary>
        /// Construtor protegido para uso por classes derivadas.
        /// </summary>
        protected EntidadeComAuditoria()
        {
            DataCriacao = DateTime.UtcNow;
            DataAtualizacao = DateTime.UtcNow;
        }

        /// <summary>
        /// Construtor protegido com ID específico para uso por classes derivadas.
        /// </summary>
        /// <param name="id">Identificador único da entidade</param>
        protected EntidadeComAuditoria(TId id)
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
        protected void AtualizarDataAlteracao(IdUsuario usuarioAlteracao)
        {
            DataAtualizacao = DateTime.UtcNow;
            UsuarioUltimaAlteracao = usuarioAlteracao;
        }
    }
}
