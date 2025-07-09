namespace Tsc.GestaoDocumentos.Domain.Services;

public interface ICriptografiaService
{
    string GerarHashSenha(string senha);
    bool VerificarSenha(string senha, string hash);
    string GerarSalt();
}
