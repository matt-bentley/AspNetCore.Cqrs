using AspNetCore.Cqrs.Application.Abstractions.Queries;
using AspNetCore.Cqrs.Application.Abstractions.Repositories;
using AspNetCore.Cqrs.Core.Locations.ReadModels;

namespace AspNetCore.Cqrs.Application.Locations.Queries
{
    public sealed record GetLocationsQuery() : Query<List<LocationReadModel>>;

    public sealed class GetLocationsQueryHandler : QueryHandler<GetLocationsQuery, List<LocationReadModel>>
    {
        private readonly ILocationsReadModelRepository _repository;

        public GetLocationsQueryHandler(ILocationsReadModelRepository repository)
        {
            _repository = repository;
        }

        protected override async Task<List<LocationReadModel>> HandleAsync(GetLocationsQuery request)
        {
            return await _repository.GetAllAsync();
        }
    }
}
