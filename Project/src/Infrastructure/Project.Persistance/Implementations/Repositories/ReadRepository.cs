using Microsoft.EntityFrameworkCore;
using Project.Application.Abstractions.Repositories;
using Project.Domain.Entities.Commons;
using Project.Persistance.Contexts;
using System.Linq.Expressions;

namespace Project.Persistance.Implementations.Repositories;

public class ReadRepository<T> : IReadRepository<T> where T : BaseAuditableEntity, new()
{
    private readonly AppDbContext _context;

    public ReadRepository(AppDbContext context)
    {
        _context = context;
    }

    public DbSet<T> Table => _context.Set<T>();

    public async Task<T> GetByIdAsync(int id, bool isTracking, params string[] includes)
    {
        IQueryable<T> query = Table.AsQueryable();
        if (!isTracking)
        {
            query = query.AsNoTracking();
        }
        if (includes.Length > 0)
        {
            foreach (string include in includes)
            {
                query = query.Include(include);
            }
        }
        T? tentity = await query.FirstOrDefaultAsync(x => x.Id == id);
        return tentity;

    }

    public async Task<ICollection<T>> GetAllAsync(bool deleted, bool isTracking, params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = Table.AsQueryable();

        if (includes.Length > 0)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        if (!isTracking)
        {
            query = query.AsNoTracking();
        }

        query = query.Where(x => x.IsDeleted == deleted);

        return await query.ToListAsync();
    }

}