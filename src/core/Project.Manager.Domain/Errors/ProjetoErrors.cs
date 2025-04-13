namespace Project.Manager.Domain.Errors;

public static class ProjetoErrors
{
    public static readonly Error IdProjetoNaoPodeSerVazio = new("Projeto.IdProjetoNaoPodeSerVazio", "O id do projeto não pode ser vazio.");
    public static readonly Error ProjetoNaoEncontrado = new("Projeto.ProjetoNaoEncontrado", "Projeto não encontrado.");
    public static readonly Error NomeProjetoNaoPodeSerVazio = new("Projeto.NomeProjetoNaoPodeSerVazio", "O campo Nome do projeto não pode ser vazio.");
    public static readonly Error DescricaoProjetoNaoPodeSerVazia = new("Projeto.NomeProjetoNaoPodeSerVazio", "O campo Descrição do projeto não pode ser vazio.");
    public static readonly Error DataInicioDeveSerMenorQueDataFim = new("Projeto.NomeProjetoNaoPodeSerVazio", "O campo Data Inicio não pode ser maior ou igual ao campo Data Fim.");
    public static readonly Error NaoExistemProjetosCadastradosParaUsuarioInformado = new("Projeto.NaoExistemProjetosCadastradosParaUsuarioInformado", "Não existem projetos cadastrados para o usuário informado.");
    public static readonly Error LimiteDeTarefasPorProjeto = new("Projeto.LimiteDeTarefasPorProjeto", "O projeto já possui 20 tarefas incluídas.");
    public static readonly Error ProjetoNaoPodeSerNulo = new("Projeto.ProjetoNaoPodeSerNulo", "O projeto não pode ser nulo.");
    public static readonly Error TarefasPendentesConclusao = new("Projeto.TarefasPendentesConclusao", "Existem tarefas pendentes de conclusão, finalize as mesmas para poder excluir o projeto.");
}