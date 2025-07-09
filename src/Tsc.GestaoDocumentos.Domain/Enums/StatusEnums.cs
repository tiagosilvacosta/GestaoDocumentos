namespace Tsc.GestaoDocumentos.Domain.Enums;

public enum StatusTenant
{
    Ativo = 1,
    Inativo = 2,
    Suspenso = 3
}

public enum StatusUsuario
{
    Ativo = 1,
    Inativo = 2
}

public enum StatusDocumento
{
    Ativo = 1,
    Inativo = 2
}

public enum PerfilUsuario
{
    Administrador = 1,
    Usuario = 2
}

public enum TipoOperacaoAuditoria
{
    CREATE = 1,
    UPDATE = 2,
    DELETE = 3,
    DOWNLOAD = 4,
    LOGIN = 5,
    LOGOUT = 6
}
