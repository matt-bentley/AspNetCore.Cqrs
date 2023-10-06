using AspNetCore.Cqrs.Application.Abstractions.Commands;
using AspNetCore.Cqrs.Application.Abstractions.Queries;

namespace AspNetCore.Cqrs.Arch.Tests
{
    [Collection("Sequential")]
    public class ApplicationLayerTests : BaseTests
    {
        [Fact]
        public void ApplicationLayer_Cqrs_QueriesEndWithQuery()
        {
            AllTypes.That().Inherit(typeof(Query<>))
            .Should().HaveNameEndingWith("Query")
            .AssertIsSuccessful();
        }

        [Fact]
        public void ApplicationLayer_Cqrs_ContainsAllQueries()
        {
            AllTypes.That().HaveNameEndingWith("Query")
            .Should().ResideInNamespace("AspNetCore.Cqrs.Application")
            .AssertIsSuccessful();
        }

        [Fact]
        public void ApplicationLayer_Cqrs_CommandsEndWithCommand()
        {
            AllTypes.That().Inherit(typeof(Command))
            .Should().HaveNameEndingWith("Command")
            .AssertIsSuccessful();
        }

        [Fact]
        public void ApplicationLayer_Cqrs_ContainsAllCommands()
        {
            AllTypes.That().HaveNameEndingWith("Command")
            .Should().ResideInNamespace("AspNetCore.Cqrs.Application")
            .AssertIsSuccessful();
        }

        [Fact]
        public void ApplicationLayer_Cqrs_QueryHandlersEndWithQueryHandler()
        {
            AllTypes.That().Inherit(typeof(QueryHandler<,>))
            .Should().HaveNameEndingWith("QueryHandler")
            .AssertIsSuccessful();
        }

        [Fact]
        public void ApplicationLayer_Cqrs_ContainsAllQueryHandlers()
        {
            AllTypes.That().HaveNameEndingWith("QueryHandler")
            .Should().ResideInNamespace("AspNetCore.Cqrs.Application")
            .AssertIsSuccessful();
        }

        [Fact]
        public void ApplicationLayer_Cqrs_CommandHandlersEndWithCommandHandler()
        {
            AllTypes.That().Inherit(typeof(CommandHandler<>))
            .Should().HaveNameEndingWith("CommandHandler")
            .AssertIsSuccessful();
        }

        [Fact]
        public void ApplicationLayer_Cqrs_ContainsAllCommandHandlers()
        {
            AllTypes.That().HaveNameEndingWith("CommandHandler")
            .Should().ResideInNamespace("AspNetCore.Cqrs.Application")
            .AssertIsSuccessful();
        }

        [Fact]
        public void ApplicationLayer_Dtos_ShouldBeMutable()
        {
            AllTypes.That().HaveNameEndingWith("Dto")
                           .And().DoNotHaveName("IntegrationSupportGroupUserDto")
            .Should().BeMutable()
            .AssertIsSuccessful();
        }
    }
}