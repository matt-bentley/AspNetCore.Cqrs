using MediatR;

namespace AspNetCore.Cqrs.Application.Abstractions.Commands
{
    public abstract record Command : IRequest<Unit>;
}
