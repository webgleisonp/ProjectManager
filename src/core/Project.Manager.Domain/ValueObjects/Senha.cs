using Project.Manager.Domain.Errors;
using Project.Manager.Domain.Shared;
using System.Text.RegularExpressions;

namespace Project.Manager.Domain.ValueObjects;

public sealed class Senha
{
    public string Value { get; init; } = null!;

    private Senha(string value)
    {
        Value = value;
    }

    public static Result<Senha> Criar(string value, string confirm)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Result.Failure<Senha>(SenhaErrors.SenhaNaoPodeSerVazia);

        if (value.Length < 8)
            return Result.Failure<Senha>(SenhaErrors.SenhaDeveTerTamanhoMinimo);

        if (!Regex.IsMatch(value, "[A-Z]"))
            return Result.Failure<Senha>(SenhaErrors.SenhaDeveTerUmaLetraMaiuscula);

        if (!Regex.IsMatch(value, "[0-9]"))
            return Result.Failure<Senha>(SenhaErrors.SenhaDeveTerUmNumero);

        if(value != confirm)
            return Result.Failure<Senha>(SenhaErrors.SenhaNaoConfere);

        return Result.Success(new Senha(value));
    }
}