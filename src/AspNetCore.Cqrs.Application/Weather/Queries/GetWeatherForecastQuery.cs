using AspNetCore.Cqrs.Application.Abstractions.Queries;
using AspNetCore.Cqrs.Application.Abstractions.Repositories;
using AspNetCore.Cqrs.Core.Abstractions.Guards;
using AspNetCore.Cqrs.Core.Weather.ReadModels;

namespace AspNetCore.Cqrs.Application.Weather.Queries
{
    public sealed record GetWeatherForecastQuery(Guid Id) : Query<WeatherForecastReadModel>;

    public sealed class GetWeatherForecastQueryHandler : QueryHandler<GetWeatherForecastQuery, WeatherForecastReadModel>
    {
        private readonly IWeatherForecastsReadModelRepository _repository;

        public GetWeatherForecastQueryHandler(IWeatherForecastsReadModelRepository repository)
        {
            _repository = repository;
        }

        protected override async Task<WeatherForecastReadModel> HandleAsync(GetWeatherForecastQuery request)
        {
            var forecast = await _repository.GetByIdAsync(request.Id);
            return Guard.Against.NotFound(forecast);
        }
    }
}
