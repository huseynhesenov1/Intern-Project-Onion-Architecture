using Project.Domain.Entities.Commons;

namespace Project.Application.Abstractions.Repositories;

public interface IWriteRepository<T> : IRepository<T> where T : BaseAuditableEntity, new()
{
    Task<T> CreateAsync(T tentity);
    T Update(T tentity);
    T SoftDelete(T tentity);
    T HardDelete(T tentity);
    T Restore(T tentity);
    Task SaveChangeAsync();
}
