using AspNetCore.Cqrs.Application.Abstractions.Queries;
using AspNetCore.Cqrs.Application.Abstractions.Repositories;
using AspNetCore.Cqrs.Core.Weather.ReadModels;

namespace AspNetCore.Cqrs.Application.Weather.Queries
{
    public sealed record GetWeatherForecastsQuery(Guid? LocationId) : Query<List<WeatherForecastReadModel>>;

    public sealed class GetWeatherForecastsQueryHandler : QueryHandler<GetWeatherForecastsQuery, List<WeatherForecastReadModel>>
    {
        private readonly IWeatherForecastsReadModelRepository _repository;

        public GetWeatherForecastsQueryHandler(IWeatherForecastsReadModelRepository repository)
        {
            _repository = repository;
        }

        protected override async Task<List<WeatherForecastReadModel>> HandleAsync(GetWeatherForecastsQuery request)
        {
            if (request.LocationId.HasValue)
            {
                return await _repository.GetByLocationAsync(request.LocationId.Value);
            }
            else
            {
                return await _repository.GetAllAsync();
            }
        }
    }
}
