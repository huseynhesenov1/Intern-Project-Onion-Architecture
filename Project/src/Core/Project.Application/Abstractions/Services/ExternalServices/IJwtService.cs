using Project.Domain.Entities;

namespace Project.Application.Abstractions.Services.ExternalServices
{
    public interface IJwtService
    {
        string GenerateToken(Worker worker);
    }
}
