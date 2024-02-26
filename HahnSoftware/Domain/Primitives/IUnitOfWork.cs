namespace Domain.Primitives;

public interface IUnitWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}