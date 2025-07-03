namespace Project.Application.Abstractions.UnitOfWork
{
    public interface IUnitOfWork
    {
        Task SaveChangesAsync();
    }
} 