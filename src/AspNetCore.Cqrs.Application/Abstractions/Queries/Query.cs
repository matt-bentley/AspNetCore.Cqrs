using MediatR;

namespace AspNetCore.Cqrs.Application.Abstractions.Queries
{
    public abstract record Query<T> : IRequest<T>;
}
