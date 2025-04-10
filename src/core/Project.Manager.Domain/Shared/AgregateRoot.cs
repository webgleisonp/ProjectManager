namespace Project.Manager.Domain.Shared;
public class AgregateRoot<T>
    where T : class
{
    public T Id { get; init; }

    public AgregateRoot(T id)
    {
        Id = id;
    }
}
