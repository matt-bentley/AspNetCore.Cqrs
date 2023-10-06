using MediatR;

namespace AspNetCore.Cqrs.Core.Abstractions.DomainEvents
{
    public abstract record DomainEvent : INotification;
}
