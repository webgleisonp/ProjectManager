namespace Project.Manager.Application.Abstractions;

public interface IUnityOfWork
{
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
