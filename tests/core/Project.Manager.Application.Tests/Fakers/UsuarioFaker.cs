using Bogus;
using Project.Manager.Domain.Entities;
using Project.Manager.Domain.ValueObjects.Identities;
using System.Text.RegularExpressions;

namespace Project.Manager.Application.Tests.Fakers;

public static class UsuarioFaker
{
    public static Faker<Usuario> GerarUsuarioFake(Guid? usuarioId)
    {
        return new Faker<Usuario>("pt_BR")
            .CustomInstantiator(f =>
            {
                var senha = f.Internet.Password(length: 8);

                // Garante que tenha ao menos 1 letra maiúscula (por segurança extra)
                if (!Regex.IsMatch(senha, "[A-Z]"))
                    senha += "A";

                // Garante que tenha ao menos 1 número (por segurança extra)
                if (!Regex.IsMatch(senha, "[0-9]"))
                    senha += "1";

                var usuarioResult = Usuario.Criar(
                    new UsuarioId(usuarioId ?? f.Random.Guid()),
                    f.Name.FullName(),
                    f.Internet.Email(),
                    senha,
                    senha
                );

                if (usuarioResult.IsFailure)
                    throw new InvalidOperationException("Erro ao criar usuario fake.");

                return usuarioResult.Value;
            });
    }
}
