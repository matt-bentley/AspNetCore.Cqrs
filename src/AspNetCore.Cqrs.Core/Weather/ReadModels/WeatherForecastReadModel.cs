using AspNetCore.Cqrs.Core.Abstractions.Entities;

namespace AspNetCore.Cqrs.Core.Weather.ReadModels
{
    public sealed class WeatherForecastReadModel : ReadModel
    {
        public DateTime Date { get; set; }
        public int TemperatureC { get; set; }
        public int TemperatureF => 32 + (int)Math.Round((TemperatureC / 0.5556), 0);
        public string? Summary { get; set; }
        public Guid LocationId { get; set; }
    }
}
