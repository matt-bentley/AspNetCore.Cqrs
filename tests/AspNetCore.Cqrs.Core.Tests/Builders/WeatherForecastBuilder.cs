using AspNetCore.Cqrs.Core.Weather.Entities;
using AspNetCore.Cqrs.Core.Weather.ReadModels;
using AspNetCore.Cqrs.Core.Weather.ValueObjects;

namespace AspNetCore.Cqrs.Core.Tests.Builders
{
    public class WeatherForecastBuilder
    {
        private Guid _id = Guid.NewGuid();
        private DateTime _date = DateTime.UtcNow;
        private int _temperature = 8;
        private string? _summary = "Mild";
        private Guid _location = new Guid("B0C91847-8931-4C45-9FD5-018A3A3398CF");

        public WeatherForecast Build()
        {
            return WeatherForecast.Create(_id, _date, Temperature.FromCelcius(_temperature), _summary, _location);
        }

        public WeatherForecastReadModel BuildReadModel()
        {
            return new WeatherForecastReadModel()
            {
                Id = _id,
                Date = _date,
                LocationId = _location,
                Summary = _summary,
                TemperatureC = _temperature
            };
        }

        public WeatherForecastBuilder WithId(Guid id)
        {
            _id = id;
            return this;
        }

        public WeatherForecastBuilder WithTemperature(int temperature)
        {
            _temperature = temperature;
            return this;
        }

        public WeatherForecastBuilder WithSummary(string? summary)
        {
            _summary = summary;
            return this;
        }

        public WeatherForecastBuilder WithDate(DateTime date)
        {
            _date = date;
            return this;
        }

        public WeatherForecastBuilder WithLocation(Guid locationId)
        {
            _location = locationId;
            return this;
        }
    }
}
