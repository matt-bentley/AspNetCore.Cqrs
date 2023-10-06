using AspNetCore.Cqrs.Infrastructure;
using AspNetCore.Cqrs.Infrastructure.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AspNetCore.Cqrs.Migrations.Factories
{
    public static class DbContextOptionsFactory
    {
        public static DbContextOptions<WeatherContext> Create(IConfiguration configuration)
        {
            var appSettings = DatabaseSettings.Create(configuration);

            return new DbContextOptionsBuilder<WeatherContext>()
                .UseSqlServer(appSettings.SqlConnectionString, b => b.MigrationsAssembly("AspNetCore.Cqrs.Migrations"))
                .Options;
        }
    }
}
