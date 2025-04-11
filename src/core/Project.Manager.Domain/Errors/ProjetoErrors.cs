namespace Project.Manager.Domain.Errors;

public static class ProjetoErrors
{
    public static Error ProjetoNaoEncontrado = new("Projeto.ProjetoNaoEncontrado", "Projeto não encontrado.");
    public static Error NomeProjetoNaoPodeSerVazio = new("Projeto.NomeProjetoNaoPodeSerVazio", "O campo Nome do projeto não pode ser vazio.");
    public static Error DescricaoProjetoNaoPodeSerVazia = new("Projeto.NomeProjetoNaoPodeSerVazio", "O campo Descrição do projeto não pode ser vazio.");
    public static Error DataInicioDeveSerMenorQueDataFim = new("Projeto.NomeProjetoNaoPodeSerVazio", "O campo Data Inicio não pode ser maior ou igual ao campo Data Fim.");
    public static Error NaoExistemProjetosCadastradosParaUsuarioInformado = new("Projeto.NaoExistemProjetosCadastradosParaUsuarioInformado", "Não existem projetos cadastrados para o usuário informado.");
    public static Error LimiteDeTarefasPorProjeto = new("Projeto.LimiteDeTarefasPorProjeto", "O projeto já possui 20 tarefas incluídas.");
}