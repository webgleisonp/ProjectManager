using Project.Manager.Domain.ValueObjects.Identities;

namespace Project.Manager.Application.Extensions;

public static class IdentityExtensions
{
    public static UsuarioId ToUsuarioId(this Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("O id não pode ser vazio.", nameof(id));

        return new UsuarioId(id);
    }

    public static ProjetoId ToProjetoId(this Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("O id não pode ser vazio.", nameof(id));

        return new ProjetoId(id);
    }

    public static TarefaId ToTarefaId(this Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("O id não pode ser vazio.", nameof(id));

        return new TarefaId(id);
    }
}
