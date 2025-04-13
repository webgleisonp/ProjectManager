using Bogus;
using Project.Manager.Domain.Entities;
using Project.Manager.Domain.ValueObjects.Enums;
using Project.Manager.Domain.ValueObjects.Identities;

namespace Project.Manager.Application.Tests.Fakers;

public static class TarefasFaker
{
    public static Faker<Tarefa> GerarTarefasFakes(Guid projetoId)
    {
        return new Faker<Tarefa>("pt_BR")
            .CustomInstantiator(f =>
            {
                var tarefaResult = Tarefa.Criar(
                    new TarefaId(f.Random.Guid()),
                    new ProjetoId(projetoId),
                    f.Lorem.Sentence(),
                    f.Lorem.Paragraph(),
                    f.Date.Past(1),
                    f.Date.Future(1),
                    StatusTarefa.Pendente,
                    PrioridadeTarefa.Baixa
                );

                if (tarefaResult.IsFailure)
                    throw new InvalidOperationException("Erro ao criar tarefa fake.");

                return tarefaResult.Value;
            });
    }
}