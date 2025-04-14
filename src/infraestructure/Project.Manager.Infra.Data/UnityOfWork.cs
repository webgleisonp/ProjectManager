using Project.Manager.Application.Abstractions;

namespace Project.Manager.Infra.Data;

public sealed class UnityOfWork(SqlServerDbContext sqlServerDbContext) : IUnityOfWork
{
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await sqlServerDbContext.SaveChangesAsync(cancellationToken);
    }
}
