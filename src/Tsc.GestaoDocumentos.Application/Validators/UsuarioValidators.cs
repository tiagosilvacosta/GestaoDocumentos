using FluentValidation;
using Tsc.GestaoDocumentos.Application.Usuarios;

namespace Tsc.GestaoDocumentos.Application.Validators;

public class CreateUsuarioDtoValidator : AbstractValidator<CreateUsuarioDto>
{
    public CreateUsuarioDtoValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("Nome é obrigatório")
            .MaximumLength(255).WithMessage("Nome não pode ter mais de 255 caracteres");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email é obrigatório")
            .EmailAddress().WithMessage("Email deve ter um formato válido")
            .MaximumLength(255).WithMessage("Email não pode ter mais de 255 caracteres");

        RuleFor(x => x.Login)
            .NotEmpty().WithMessage("Login é obrigatório")
            .MaximumLength(100).WithMessage("Login não pode ter mais de 100 caracteres");

        RuleFor(x => x.Senha)
            .NotEmpty().WithMessage("Senha é obrigatória")
            .MinimumLength(8).WithMessage("Senha deve ter no mínimo 8 caracteres")
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]")
            .WithMessage("Senha deve conter ao menos: 1 letra minúscula, 1 maiúscula, 1 número e 1 caractere especial");

        RuleFor(x => x.Perfil)
            .NotEmpty().WithMessage("Perfil é obrigatório")
            .Must(BeValidPerfil).WithMessage("Perfil deve ser Administrador ou Usuario");
    }

    private static bool BeValidPerfil(string perfil)
    {
        return perfil is "Administrador" or "Usuario";
    }
}

public class UpdateUsuarioDtoValidator : AbstractValidator<UpdateUsuarioDto>
{
    public UpdateUsuarioDtoValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("Nome é obrigatório")
            .MaximumLength(255).WithMessage("Nome não pode ter mais de 255 caracteres");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email é obrigatório")
            .EmailAddress().WithMessage("Email deve ter um formato válido")
            .MaximumLength(255).WithMessage("Email não pode ter mais de 255 caracteres");

        RuleFor(x => x.Login)
            .NotEmpty().WithMessage("Login é obrigatório")
            .MaximumLength(100).WithMessage("Login não pode ter mais de 100 caracteres");

        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("Status é obrigatório")
            .Must(BeValidStatus).WithMessage("Status deve ser Ativo ou Inativo");

        RuleFor(x => x.Perfil)
            .NotEmpty().WithMessage("Perfil é obrigatório")
            .Must(BeValidPerfil).WithMessage("Perfil deve ser Administrador ou Usuario");
    }

    private static bool BeValidStatus(string status)
    {
        return status is "Ativo" or "Inativo";
    }

    private static bool BeValidPerfil(string perfil)
    {
        return perfil is "Administrador" or "Usuario";
    }
}

public class AlterarSenhaDtoValidator : AbstractValidator<AlterarSenhaDto>
{
    public AlterarSenhaDtoValidator()
    {
        RuleFor(x => x.SenhaAtual)
            .NotEmpty().WithMessage("Senha atual é obrigatória");

        RuleFor(x => x.NovaSenha)
            .NotEmpty().WithMessage("Nova senha é obrigatória")
            .MinimumLength(8).WithMessage("Nova senha deve ter no mínimo 8 caracteres")
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]")
            .WithMessage("Nova senha deve conter ao menos: 1 letra minúscula, 1 maiúscula, 1 número e 1 caractere especial");

        RuleFor(x => x.ConfirmarNovaSenha)
            .NotEmpty().WithMessage("Confirmação da nova senha é obrigatória")
            .Equal(x => x.NovaSenha).WithMessage("Confirmação da nova senha deve ser igual à nova senha");
    }
}
