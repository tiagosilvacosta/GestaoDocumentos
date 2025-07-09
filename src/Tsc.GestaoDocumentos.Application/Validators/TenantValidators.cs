using FluentValidation;
using Tsc.GestaoDocumentos.Application.DTOs;

namespace Tsc.GestaoDocumentos.Application.Validators;

public class CreateTenantDtoValidator : AbstractValidator<CreateTenantDto>
{
    public CreateTenantDtoValidator()
    {
        RuleFor(x => x.NomeOrganizacao)
            .NotEmpty().WithMessage("Nome da organização é obrigatório")
            .MaximumLength(255).WithMessage("Nome da organização não pode ter mais de 255 caracteres");

        RuleFor(x => x.Slug)
            .NotEmpty().WithMessage("Slug é obrigatório")
            .MaximumLength(50).WithMessage("Slug não pode ter mais de 50 caracteres")
            .Matches(@"^[a-z0-9-]+$").WithMessage("Slug deve conter apenas letras minúsculas, números e hífens");

        RuleFor(x => x.DataExpiracao)
            .GreaterThan(DateTime.UtcNow).WithMessage("Data de expiração deve ser futura")
            .When(x => x.DataExpiracao.HasValue);
    }
}

public class UpdateTenantDtoValidator : AbstractValidator<UpdateTenantDto>
{
    public UpdateTenantDtoValidator()
    {
        RuleFor(x => x.NomeOrganizacao)
            .NotEmpty().WithMessage("Nome da organização é obrigatório")
            .MaximumLength(255).WithMessage("Nome da organização não pode ter mais de 255 caracteres");

        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("Status é obrigatório")
            .Must(BeValidStatus).WithMessage("Status deve ser Ativo, Inativo ou Suspenso");

        RuleFor(x => x.DataExpiracao)
            .GreaterThan(DateTime.UtcNow).WithMessage("Data de expiração deve ser futura")
            .When(x => x.DataExpiracao.HasValue);
    }

    private bool BeValidStatus(string status)
    {
        return status is "Ativo" or "Inativo" or "Suspenso";
    }
}
