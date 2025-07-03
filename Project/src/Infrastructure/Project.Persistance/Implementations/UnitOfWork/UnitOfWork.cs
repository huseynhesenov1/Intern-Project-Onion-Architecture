using Project.Application.Abstractions.UnitOfWork;
using Project.Persistance.Contexts;

namespace Project.Persistance.Implementations.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
} 