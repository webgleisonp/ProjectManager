namespace Project.Manager.Domain.Errors;

public static class SenhaErrors
{
    public static Error SenhaNaoPodeSerVazia => new("Senha.NaoPodeSerVazia", "O campo Senha não pode ser vazio.");
    public static Error SenhaDeveTerTamanhoMinimo => new("Senha.SenhaDeveTerTamanhoMinimo", "Senha deve ter no mínimo 8 caracteres.");
    public static Error SenhaDeveTerUmaLetraMaiuscula = new("Senha.SenhaDeveTerUmaLetraMaiuscula", "Senha deve conter pelo menos uma letra maiúscula.");
    public static Error SenhaDeveTerUmNumero = new("Senha.SenhaDeveTerUmNumero", "Senha deve conter pelo menos um número.");
    public static Error SenhaNaoConfere = new("Senha.SenhaNaoConfere", "As senhas não conferem.");
}