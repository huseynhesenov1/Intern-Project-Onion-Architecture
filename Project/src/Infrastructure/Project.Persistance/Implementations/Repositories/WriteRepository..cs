using Microsoft.EntityFrameworkCore;
using Project.Application.Abstractions.Repositories;
using Project.Domain.Entities.Commons;
using Project.Persistance.Contexts;

namespace Project.Persistance.Implementations.Repositories;

public class WriteRepository<T> : IWriteRepository<T> where T : BaseAuditableEntity, new()
{
    private readonly AppDbContext _context;
    public WriteRepository(AppDbContext context)
    {
        _context = context;
    }
    public DbSet<T> Table => _context.Set<T>();
    public async Task<T> CreateAsync(T tentity)
    {
        await Table.AddAsync(tentity);
        return tentity;
    }

    public T SoftDelete(T tentity)
    {
        if (tentity is null)
        {
            throw new Exception("Bu Id ye uygun deyer tapilmadi");
        }
        tentity.IsDeleted = true;
        return tentity;
    }
    

    public T Restore(T tentity)
    {
        if (tentity is null)
        {
            throw new Exception("Bu Id-e uygun deyer tapilmadi");
        }
        tentity.IsDeleted = false;
        return tentity;
    }

    public async Task SaveChangeAsync()
    {
        await _context.SaveChangesAsync();
    }

    public T Update(T tentity)
    {
        Table.Update(tentity);
        return tentity;
    }
}
