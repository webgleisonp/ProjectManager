﻿namespace Project.Manager.Domain.Errors;

public static class TarefaErrors
{
    public static Error NomeTarefaNaoPodeSerVazio=new("Tarefa.NomeTarefaNaoPodeSerVazio", "O campo Nome da tarefa não pode ser vazio.");
    public static Error DescricaoTarefaNaoPodeSerVazia = new("Tarefa.DescricaoTarefaNaoPodeSerVazia", "O campo Descrição da tarefa não pode ser vazio");
    public static Error DataInicioDeveSerMenorQueDataFim = new("Tarefa.DataInicioDeveSerMenorQueDataFim", "O campo Data Inicio não pode ser maior ou igual ao campo Data Fim.");
}