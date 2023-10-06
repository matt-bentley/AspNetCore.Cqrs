using AspNetCore.Cqrs.Core.Locations.ReadModels;

namespace AspNetCore.Cqrs.Application.Abstractions.Repositories
{
    public interface ILocationsReadModelRepository : IReadModelRepository<LocationReadModel>
    {
    }
}
