using AspNetCore.Cqrs.Application.Abstractions.Repositories;
using MediatR;

namespace AspNetCore.Cqrs.Application.Abstractions.Commands
{
    public abstract class CommandHandler<TCommand> : IRequestHandler<TCommand, Unit> where TCommand : Command
    {
        protected readonly IUnitOfWork UnitOfWork;

        protected CommandHandler(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(TCommand request, CancellationToken cancellationToken)
        {
            await HandleAsync(request);
            return Unit.Value;
        }

        protected abstract Task HandleAsync(TCommand request);
    }
}
