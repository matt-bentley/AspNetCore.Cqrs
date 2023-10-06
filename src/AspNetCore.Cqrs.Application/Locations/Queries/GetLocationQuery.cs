using AspNetCore.Cqrs.Application.Abstractions.Queries;
using AspNetCore.Cqrs.Application.Abstractions.Repositories;
using AspNetCore.Cqrs.Core.Abstractions.Guards;
using AspNetCore.Cqrs.Core.Locations.ReadModels;

namespace AspNetCore.Cqrs.Application.Locations.Queries
{
    public sealed record GetLocationQuery(Guid Id) : Query<LocationReadModel>;

    public sealed class GetLocationQueryHandler : QueryHandler<GetLocationQuery, LocationReadModel>
    {
        private readonly ILocationsReadModelRepository _repository;

        public GetLocationQueryHandler(ILocationsReadModelRepository repository)
        {
            _repository = repository;
        }

        protected override async Task<LocationReadModel> HandleAsync(GetLocationQuery request)
        {
            var location = await _repository.GetByIdAsync(request.Id);
            return Guard.Against.NotFound(location);
        }
    }
}
