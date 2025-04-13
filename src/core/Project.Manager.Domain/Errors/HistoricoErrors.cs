namespace Project.Manager.Domain.Errors;

public static class HistoricoErrors
{
    public static readonly Error IdHistoricoNaoPodeSerVazio = new("Historico.IdHistoricoNaoPodeSerVazio", "O id do histórico não pode ser vazio.");
    public static readonly Error DescricaoHistoricoNaoPodeSerVazia = new("Historico.DescricaoHistoricoNaoPodeSerVazia", "A descrição do histórico não pode ser vazia.");
}
