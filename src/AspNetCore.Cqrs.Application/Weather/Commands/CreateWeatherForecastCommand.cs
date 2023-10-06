using AspNetCore.Cqrs.Application.Abstractions.Commands;
using AspNetCore.Cqrs.Application.Abstractions.Repositories;
using AspNetCore.Cqrs.Core.Abstractions.Guards;
using AspNetCore.Cqrs.Core.Weather.Entities;
using AspNetCore.Cqrs.Core.Weather.ValueObjects;

namespace AspNetCore.Cqrs.Application.Weather.Commands
{
    public sealed record CreateWeatherForecastCommand(Guid Id, int Temperature, DateTime Date, string? Summary, Guid LocationId) : Command;

    public sealed class CreateWeatherForecastCommandHandler : CommandHandler<CreateWeatherForecastCommand>
    {
        private readonly IRepository<WeatherForecast> _repository;
        private readonly ILocationsReadModelRepository _locationsRepository;

        public CreateWeatherForecastCommandHandler(IRepository<WeatherForecast> repository,
            ILocationsReadModelRepository locationsRepository,
            IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _repository = repository;
            _locationsRepository = locationsRepository;
        }

        protected override async Task HandleAsync(CreateWeatherForecastCommand request)
        {
            var location = await _locationsRepository.GetByIdAsync(request.LocationId);
            location = Guard.Against.NotFound(location, $"Location not found: {request.LocationId}");

            var created = WeatherForecast.Create(request.Id,
                                                 request.Date,
                                                 Temperature.FromCelcius(request.Temperature),
                                                 request.Summary,
                                                 location.Id);
            _repository.Insert(created);
            await UnitOfWork.CommitAsync();
        }
    }
}
