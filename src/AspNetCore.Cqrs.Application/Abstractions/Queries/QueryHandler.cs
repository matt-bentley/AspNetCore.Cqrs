using MediatR;

namespace AspNetCore.Cqrs.Application.Abstractions.Queries
{
    public abstract class QueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, TResponse> where TQuery : Query<TResponse>
    {
        public async Task<TResponse> Handle(TQuery request, CancellationToken cancellationToken)
        {
            return await HandleAsync(request);
        }

        protected abstract Task<TResponse> HandleAsync(TQuery request);
    }
}
