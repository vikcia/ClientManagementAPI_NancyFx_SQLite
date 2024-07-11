using Domain.Dtos;
using FluentValidation.Results;

namespace Domain.Interfaces;

public interface IClientDtoValidator
{
    ValidationResult Validate(ClientDto client);
}