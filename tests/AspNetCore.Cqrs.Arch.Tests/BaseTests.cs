using AspNetCore.Cqrs.Application.AutofacModules;
using AspNetCore.Cqrs.Core.Abstractions.Entities;
using AspNetCore.Cqrs.Infrastructure.AutofacModules;
using System.Reflection;

namespace AspNetCore.Cqrs.Arch.Tests
{
    public abstract class BaseTests
    {
        protected static Assembly ApiAssembly = typeof(Api.Controllers.WeatherForecastsController).Assembly;
        protected static Assembly ApplicationAssembly = typeof(ApplicationModule).Assembly;
        protected static Assembly InfrastuctureAssembly = typeof(InfrastructureModule).Assembly;
        protected static Assembly CoreAssembly = typeof(EntityBase).Assembly;
        protected static Types AllTypes = Types.InAssemblies(new List<Assembly> { ApiAssembly, ApplicationAssembly, InfrastuctureAssembly, CoreAssembly });
    }
}
