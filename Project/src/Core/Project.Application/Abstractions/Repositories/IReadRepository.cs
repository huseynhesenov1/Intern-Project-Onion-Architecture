using Project.Domain.Entities.Commons;

namespace Project.Application.Abstractions.Repositories;

public interface IReadRepository<T> : IRepository<T> where T : BaseAuditableEntity, new()
{
    Task<T> GetByIdAsync(int id, bool isTracking, params string[] includes);
    Task<ICollection<T>> GetAllAsync(bool deleted, params string[] includes);
}