using AspNetCore.Cqrs.Core.Abstractions.Entities;
using AspNetCore.Cqrs.Application.Abstractions.Repositories;
using MockQueryable.Moq;

namespace AspNetCore.Cqrs.Core.Tests.Factories
{
    public static class MockRepositoryFactory
    {
        public static Mock<TRepository> Create<T, TRepository>(IEnumerable<T>? items = null)
            where T : AggregateRoot
            where TRepository : class, IRepository<T>
        {
            var repository = new Mock<TRepository>();
            return Setup(repository, items);
        }

        public static Mock<IRepository<T>> Create<T>(IEnumerable<T>? items = null)
        where T : AggregateRoot
        {
            var repository = new Mock<IRepository<T>>();
            return Setup(repository, items);
        }

        public static Mock<TRepository> CreateReadModel<T, TRepository>(IEnumerable<T>? items = null)
            where T : ReadModel
             where TRepository : class, IReadModelRepository<T>
        {
            var repository = new Mock<TRepository>();
            return SetupReadModel(repository, items);
        }

        public static Mock<TRepository> Setup<T, TRepository>(Mock<TRepository> repository, IEnumerable<T>? items = null)
            where T : AggregateRoot
            where TRepository : class, IRepository<T>
        {
            if (items == null)
            {
                items = new List<T>();
            }
            repository.Setup(e => e.GetByIdAsync(It.IsAny<Guid>())).Returns<Guid>((id) => Task.FromResult(items.FirstOrDefault(e => e.Id == id)));
            repository.Setup(e => e.GetAll(It.IsAny<bool>())).Returns(() => items.AsQueryable().BuildMockDbSet().Object);
            return repository;
        }

        public static Mock<TRepository> SetupReadModel<T, TRepository>(Mock<TRepository> repository, IEnumerable<T>? items = null)
            where T : ReadModel
            where TRepository : class, IReadModelRepository<T>
        {
            if (items == null)
            {
                items = new List<T>();
            }
            repository.Setup(e => e.GetByIdAsync(It.IsAny<Guid>())).Returns<Guid>((id) => Task.FromResult(items.FirstOrDefault(e => e.Id == id)));
            repository.Setup(e => e.GetAllAsync()).ReturnsAsync(() => items.ToList());
            return repository;
        }
    }
}
