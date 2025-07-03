using Project.Domain.Entities.Commons;
using System.Linq.Expressions;

namespace Project.Application.Abstractions.Repositories;

public interface IReadRepository<T> : IRepository<T> where T : BaseAuditableEntity, new()
{
    Task<T> GetByIdAsync(int id,  params Expression<Func<T, object>>[] includes);
    Task<ICollection<T>> GetAllAsync(bool isTracking, params Expression<Func<T, object>>[] includes);

}