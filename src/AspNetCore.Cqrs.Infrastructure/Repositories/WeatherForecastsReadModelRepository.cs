using AspNetCore.Cqrs.Application.Abstractions.Repositories;
using AspNetCore.Cqrs.Core.Weather.ReadModels;
using Dapper;

namespace AspNetCore.Cqrs.Infrastructure.Repositories
{
    public sealed class WeatherForecastsReadModelRepository : IWeatherForecastsReadModelRepository
    {
        private readonly DapperContext _context;

        public WeatherForecastsReadModelRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<List<WeatherForecastReadModel>> GetAllAsync()
        {
            var query = "SELECT * FROM WeatherForecasts ORDER BY Date";
            using var connection = _context.CreateConnection();
            var locations = await connection.QueryAsync<WeatherForecastReadModel>(query);
            return locations.ToList();
        }

        public async Task<List<WeatherForecastReadModel>> GetByLocationAsync(Guid locationId)
        {
            var query = "SELECT * FROM WeatherForecasts WHERE LocationId = @LocationId ORDER BY Date";
            using var connection = _context.CreateConnection();
            var locations = await connection.QueryAsync<WeatherForecastReadModel>(query, new { locationId });
            return locations.ToList();
        }

        public async Task<WeatherForecastReadModel?> GetByIdAsync(Guid id)
        {
            var query = "SELECT TOP 1 * FROM WeatherForecasts WHERE Id = @Id";
            using var connection = _context.CreateConnection();
            var location = await connection.QueryFirstOrDefaultAsync<WeatherForecastReadModel>(query, new { id });
            return location;
        }
    }
}
