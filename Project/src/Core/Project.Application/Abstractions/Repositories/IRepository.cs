using Microsoft.EntityFrameworkCore;
using Project.Domain.Entities.Commons;

namespace Project.Application.Abstractions.Repositories;

public interface IRepository<T> where T : BaseAuditableEntity, new()
{
    public DbSet<T> Table { get; }
}