using AspNetCore.Cqrs.Core.Weather.ReadModels;

namespace AspNetCore.Cqrs.Application.Abstractions.Repositories
{
    public interface IWeatherForecastsReadModelRepository : IReadModelRepository<WeatherForecastReadModel>
    {
        Task<List<WeatherForecastReadModel>> GetByLocationAsync(Guid locationId);
    }
}
