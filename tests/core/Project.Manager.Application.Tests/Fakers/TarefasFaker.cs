using Bogus;
using Project.Manager.Application.Extensions;
using Project.Manager.Domain.Entities;
using Project.Manager.Domain.ValueObjects.Enums;

namespace Project.Manager.Application.Tests.Fakers;

public static class TarefasFaker
{
    public static Faker<Tarefa> GerarTarefasFakes()
    {
        return new Faker<Tarefa>("pt_BR")
            .CustomInstantiator(f =>
            {
                var id = f.Random.Guid();
                var projetoId = f.Random.Guid();
                var usuarioId = f.Random.Guid();

                var tarefaResult = Tarefa.Criar(id.ToTarefaId(),
                    projetoId.ToProjetoId(),
                    usuarioId.ToUsuarioId(),
                    f.Lorem.Sentence(),
                    f.Lorem.Paragraph(),
                    f.Date.Past(1),
                    f.Date.Future(1),
                    f.PickRandom<StatusTarefa>(),
                    f.PickRandom<PrioridadeTarefa>()
                );

                if (tarefaResult.IsFailure)
                    throw new InvalidOperationException("Erro ao criar tarefa fake.");

                return tarefaResult.Value;
            });
    }

    public static Faker<Tarefa> GerarTarefasFakes(Guid projetoId)
    {
        return new Faker<Tarefa>("pt_BR")
            .CustomInstantiator(f =>
            {
                var id = f.Random.Guid();
                var usuarioId = f.Random.Guid();

                var tarefaResult = Tarefa.Criar(id.ToTarefaId(),
                    projetoId.ToProjetoId(),
                    usuarioId.ToUsuarioId(),
                    f.Lorem.Sentence(),
                    f.Lorem.Paragraph(),
                    f.Date.Past(1),
                    f.Date.Future(1),
                    f.PickRandom<StatusTarefa>(),
                    f.PickRandom<PrioridadeTarefa>()
                );

                if (tarefaResult.IsFailure)
                    throw new InvalidOperationException("Erro ao criar tarefa fake.");

                return tarefaResult.Value;
            });
    }

    public static Faker<Tarefa> GerarTarefasStatusFakes(Guid projetoId, StatusTarefa status)
    {
        return new Faker<Tarefa>("pt_BR")
            .CustomInstantiator(f =>
            {
                var id = f.Random.Guid();
                var usuarioId = f.Random.Guid();

                var tarefaResult = Tarefa.Criar(id.ToTarefaId(),
                    projetoId.ToProjetoId(),
                    usuarioId.ToUsuarioId(),
                    f.Lorem.Sentence(),
                    f.Lorem.Paragraph(),
                    f.Date.Past(1),
                    f.Date.Future(1),
                    status,
                    f.PickRandom<PrioridadeTarefa>()
                );

                if (tarefaResult.IsFailure)
                    throw new InvalidOperationException("Erro ao criar tarefa fake.");

                return tarefaResult.Value;
            });
    }
}