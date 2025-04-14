using Bogus;
using Project.Manager.Application.Extensions;
using Project.Manager.Domain.Entities;

namespace Project.Manager.Application.Tests.Fakers;

public static class HistoricoFaker
{
    public static Faker<Historico> GerarHistoricoFake()
    {
        return new Faker<Historico>("pt_BR")
            .CustomInstantiator(f =>
            {
                var id = f.Random.Guid();

                var tarefa = TarefasFaker.GerarTarefasFakes().Generate();

                if(tarefa is null)
                    throw new InvalidOperationException("Erro ao criar tarefa fake.");

                var usuario = UsuarioFaker.GerarUsuarioFake().Generate();

                var historicoResult = Historico.CriarComentario(id.ToHistorioId(),
                    tarefa.Id,
                    usuario.Id,
                    f.Lorem.Sentence()
                );

                if (historicoResult.IsFailure)
                    throw new InvalidOperationException("Erro ao criar historico fake.");

                var historico = historicoResult.Value;

                historico.SetTarefa(tarefa);

                historico.SetUsuario(usuario);

                return historicoResult.Value;
            });
    }
}