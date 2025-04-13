using Bogus;
using Project.Manager.Domain.Entities;
using Project.Manager.Domain.ValueObjects.Identities;

namespace Project.Manager.Application.Tests.Fakers;

public static class ProjetosFaker
{
    public static Faker<Projeto> GerarProjetosFakes(ProjetoId projetoId)
    {
        return new Faker<Projeto>("pt_BR")
            .CustomInstantiator(f =>
            {
                var usuario = UsuarioFaker.GerarUsuarioFake(f.Random.Guid()).Generate();

                var projetoResult = Projeto.Criar(
                    projetoId,
                    usuario.Id,
                    f.Lorem.Sentence(),
                    f.Lorem.Paragraph(),
                    f.Date.Past(1),
                    f.Date.Future(1)
                );

                if (projetoResult.IsFailure)
                    throw new InvalidOperationException("Erro ao criar projeto fake.");

                var projeto = projetoResult.Value;
                projeto.SetUsuario(usuario);

                return projeto;
            });
    }

    public static Faker<Projeto> GerarProjetosFakes(Guid usuarioId)
    {
        return new Faker<Projeto>("pt_BR")
            .CustomInstantiator(f =>
            {
                var usuario = UsuarioFaker.GerarUsuarioFake(usuarioId).Generate();

                var projetoResult = Projeto.Criar(
                    new ProjetoId(f.Random.Guid()),
                    usuario.Id,
                    f.Lorem.Sentence(),
                    f.Lorem.Paragraph(),
                    f.Date.Past(1),
                    f.Date.Future(1)
                );

                if (projetoResult.IsFailure)
                    throw new InvalidOperationException("Erro ao criar projeto fake.");

                var projeto = projetoResult.Value;
                projeto.SetUsuario(usuario);

                return projeto;
            });
    }
}