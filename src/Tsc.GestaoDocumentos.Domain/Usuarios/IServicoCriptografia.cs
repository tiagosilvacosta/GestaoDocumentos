namespace Tsc.GestaoDocumentos.Domain.Usuarios;

public interface IServicoCriptografia
{
    string GerarHashSenha(string senha);
    bool VerificarSenha(string senha, string hash);
    string GerarSalt();
}
