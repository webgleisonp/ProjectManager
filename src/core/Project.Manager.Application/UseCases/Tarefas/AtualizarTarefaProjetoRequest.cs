﻿using Project.Manager.Domain.ValueObjects.Enums;

namespace Project.Manager.Application.UseCases.Tarefas;

public sealed record AtualizarTarefaProjetoRequest(
    string Nome,
    string Descricao,
    DateOnly DataInicio,
    DateOnly DataFim,
    StatusTarefa Status);
