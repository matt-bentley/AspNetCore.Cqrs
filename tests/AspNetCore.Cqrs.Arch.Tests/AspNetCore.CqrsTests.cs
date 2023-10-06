using AspNetCore.Cqrs.Application.Abstractions.Repositories;

namespace AspNetCore.Cqrs.Arch.Tests
{
    [Collection("Sequential")]
    public class CqrsTests : BaseTests
    {
        [Fact]
        public void Cqrs_Layers_ApplicationDoesNotReferenceInfrastructure()
        {
            AllTypes.That().ResideInNamespace("AspNetCore.Cqrs.Application")
            .ShouldNot().HaveDependencyOn("AspNetCore.Cqrs.Infrastructure")
            .AssertIsSuccessful();
        }

        [Fact]
        public void Cqrs_Layers_CoreDoesNotReferenceOuter()
        {
            var coreTypes = AllTypes.That().ResideInNamespace("AspNetCore.Cqrs.Core");

            coreTypes.ShouldNot().HaveDependencyOn("AspNetCore.Cqrs.Infrastructure")
                .AssertIsSuccessful();

            coreTypes.ShouldNot().HaveDependencyOn("AspNetCore.Cqrs.Application")
                .AssertIsSuccessful();
        }

        [Fact]
        public void Cqrs_Repositories_OnlyInInfrastructure()
        {
            AllTypes.That().HaveNameEndingWith("Repository")
                .And().DoNotHaveNameMatching(".ReadModelRepository")
                .Should().ResideInNamespaceStartingWith("AspNetCore.Cqrs.Infrastructure")
                .AssertIsSuccessful();

            AllTypes.That().HaveNameEndingWith("Repository")
                .And().DoNotHaveNameMatching(".ReadModelRepository")
                .And().AreClasses()
                .Should().ImplementInterface(typeof(IRepository<>))
                .AssertIsSuccessful();

            AllTypes.That().HaveNameEndingWith("ReadModelRepository")
                .And().AreClasses()
                .Should().ImplementInterface(typeof(IReadModelRepository<>))
                .AssertIsSuccessful();
        }

        [Fact]
        public void Cqrs_Repositories_ShouldEndWithRepository()
        {
            AllTypes.That().Inherit(typeof(IRepository<>))
                .Should().HaveNameEndingWith("Repository")
                .AssertIsSuccessful();
        }
    }
}