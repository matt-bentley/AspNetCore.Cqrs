using AspNetCore.Cqrs.Core.Abstractions.Entities;

namespace AspNetCore.Cqrs.Application.Abstractions.Repositories
{
    public interface IReadModelRepository<T> where T : ReadModel
    {
        Task<T?> GetByIdAsync(Guid id);
        Task<List<T>> GetAllAsync();
    }
}
