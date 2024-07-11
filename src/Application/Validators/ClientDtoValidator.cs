using Domain.Dtos;
using Domain.Interfaces;
using FluentValidation;

namespace Application.Validators;

public class ClientDtoValidator : AbstractValidator<ClientDto>, IClientDtoValidator
{
    public ClientDtoValidator()
    {
        RuleFor(value => value.Name)
            .NotEmpty().WithMessage("Name is required.")
            .Must(value => value == null || IsString(value)).WithMessage("Comment must be a string.");

        RuleFor(value => value.Age)
            .NotEmpty().WithMessage("Age is required.")
            .GreaterThan(0).WithMessage("Age must be greater than 0.");

        RuleFor(value => value.Comment)
                .NotEmpty().WithMessage("Comment is required.")
                .Must(value => value == null || IsString(value)).WithMessage("Comment must be a string.");
    }

    private bool IsString(string value)
    {
        return !int.TryParse(value, out _);
    }
}