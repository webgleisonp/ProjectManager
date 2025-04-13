namespace Project.Manager.Domain.Errors;

public static class TarefaErrors
{
    public static readonly Error IdTarefaNaoPodeSerVazio = new("Tarefa.IdTarefaNaoPodeSerVazio", "O id da tarefa não pode ser vazio.");
    public static readonly Error NomeTarefaNaoPodeSerVazio = new("Tarefa.NomeTarefaNaoPodeSerVazio", "O campo Nome da tarefa não pode ser vazio.");
    public static readonly Error DescricaoTarefaNaoPodeSerVazia = new("Tarefa.DescricaoTarefaNaoPodeSerVazia", "O campo Descrição da tarefa não pode ser vazio");
    public static readonly Error DataInicioDeveSerMenorQueDataFim = new("Tarefa.DataInicioDeveSerMenorQueDataFim", "O campo Data Inicio não pode ser maior ou igual ao campo Data Fim.");
    public static readonly Error NenhumaTarefaEncontradaParaProjetoInformado = new("Tarefa.NenhumaTarefaEncontradaParaProjetoInformado", "Nenhuma tarefa encontrada para o projeto informado.");
    public static readonly Error TarefaNaoEncontrada = new("Tarefa.TarefaNaoEncontrada", "Tarefa não encontrada");
    public static readonly Error TarefaJaIniciada = new("Tarefa.TarefaJaIniciada", "Tarefa já iniciada");
}