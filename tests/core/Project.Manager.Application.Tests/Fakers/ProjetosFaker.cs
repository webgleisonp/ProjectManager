using Bogus;
using Project.Manager.Application.Extensions;
using Project.Manager.Domain.Entities;

namespace Project.Manager.Application.Tests.Fakers;

public static class ProjetosFaker
{
    public static Faker<Projeto> GerarProjetosFakes()
    {
        return new Faker<Projeto>("pt_BR")
            .CustomInstantiator(f =>
            {
                var id = f.Random.Guid();
                var usuario = UsuarioFaker.GerarUsuarioFake(f.Random.Guid()).Generate();

                var projetoResult = Projeto.Criar(id.ToProjetoId(),
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
                var id = f.Random.Guid();
                var usuario = UsuarioFaker.GerarUsuarioFake(usuarioId).Generate();

                var projetoResult = Projeto.Criar(id.ToProjetoId(),
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