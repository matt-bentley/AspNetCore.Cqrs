using AspNetCore.Cqrs.Core.Abstractions.DomainEvents;

namespace AspNetCore.Cqrs.Core.Weather.DomainEvents
{
    public sealed record WeatherForecastCreatedDomainEvent(Guid Id, int Temperature, string Summary, DateTime Date) : DomainEvent;
}
