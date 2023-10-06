using Autofac;
using AspNetCore.Cqrs.Application.Abstractions.Repositories;
using AspNetCore.Cqrs.Infrastructure.Repositories;
using AspNetCore.Cqrs.Infrastructure.Services;
using AspNetCore.Cqrs.Infrastructure.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace AspNetCore.Cqrs.Infrastructure.AutofacModules
{
    public sealed class InfrastructureModule : Module
    {
        private readonly DbContextOptions<WeatherContext> _options;
        private readonly IConfiguration Configuration;

        public InfrastructureModule(IConfiguration configuration) : this(CreateDbOptions(configuration), configuration)
        {

        }

        public InfrastructureModule(DbContextOptions<WeatherContext> options, IConfiguration configuration)
        {
            Configuration = configuration;
            _options = options;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(Options.Create(DatabaseSettings.Create(Configuration)));
            builder.RegisterType<WeatherContext>()
                .AsSelf()
                .InstancePerRequest()
                .InstancePerLifetimeScope()
                .WithParameter(new NamedParameter("options", _options));

            builder.RegisterType<UnitOfWork>()
                .AsImplementedInterfaces()
                .InstancePerRequest()
                .InstancePerLifetimeScope();

            builder.RegisterType<DapperContext>()
                .AsSelf()
                .SingleInstance();

            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(e => e.Name.EndsWith("ReadModelRepository"))
                .AsImplementedInterfaces()
                .InstancePerRequest()
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(Repository<>))
                .As(typeof(IRepository<>));

            builder.RegisterType<NotificationsService>()
                .AsImplementedInterfaces()
                .SingleInstance();
        }

        private static DbContextOptions<WeatherContext> CreateDbOptions(IConfiguration configuration)
        {
            var databaseSettings = DatabaseSettings.Create(configuration);
            var builder = new DbContextOptionsBuilder<WeatherContext>();
            builder.UseSqlServer(databaseSettings.SqlConnectionString);
            return builder.Options;
        }
    }
}
