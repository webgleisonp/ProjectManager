using Project.Manager.Domain.Errors;
using Project.Manager.Domain.Shared;

namespace Project.Manager.Domain.ValueObjects;

public sealed class Email
{
    public string Value { get; init; } = null!;

    private Email(string value)
    {
        Value = value;
    }

    public static Result<Email> Criar(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Result.Failure<Email>(EmailErrors.EmailNaoPodeSerVazio);

        if (!value.Contains("@"))
            return Result.Failure<Email>(EmailErrors.EmailInvalido);

        return new Email(value)!;
    }
}