namespace Domain.CustomException;

public class BadRequestException : Exception
{
    public string? ValidationErrors { get; }

    public BadRequestException(string message) : base(message)
    {
    }

    public BadRequestException(string message, string validationErrors) : base(message)
    {
        ValidationErrors = validationErrors;
    }
}
