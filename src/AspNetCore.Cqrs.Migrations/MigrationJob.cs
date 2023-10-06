using AspNetCore.Cqrs.Core.Locations.ValueObjects;
using AspNetCore.Cqrs.Hosting;
using AspNetCore.Cqrs.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using AspNetCore.Cqrs.Core.Locations.Entities;

namespace AspNetCore.Cqrs.Migrations
{
    public sealed class MigrationJob : Job
    {
        private readonly WeatherContext _context;

        public MigrationJob(ILogger<MigrationJob> logger,
            WeatherContext context,
            IHostApplicationLifetime hostApplicationLifetime) : base(logger, hostApplicationLifetime)
        {
            _context = context;
        }

        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            await MigrateDatabaseAsync();
        }

        private async Task MigrateDatabaseAsync()
        {
            Logger.LogInformation("Starting database migration");
            await _context.Database.MigrateAsync();
            Logger.LogInformation("Finished database migration");
            await MigrateLocationsAsync();
        }

        private async Task MigrateLocationsAsync()
        {
            var locations = new List<Location>()
            {
                CreateLocation("United Kingdom", "London", 51.51m, -0.13m),
                CreateLocation("India", "Mumbai", 17.38m, -78.46m),
                CreateLocation("USA", "New York", 40.71m, -74.01m),
                CreateLocation("Japan", "Tokyo", 35.69m, 139.69m),
                CreateLocation("Australia", "Sydney", -33.87m, 151.21m)
            };
            var existingLocations = _context.Locations.ToList();
            foreach (var location in locations)
            {
                if (!existingLocations.Any(e => e.City == location.City))
                {
                    Logger.LogInformation("Adding location: {city}", location.City);
                    _context.Locations.Add(location);
                    await _context.SaveChangesAsync();
                }
            }
        }

        private Location CreateLocation(string country, string city, decimal latitude, decimal longitude)
        {
            return Location.Create(country, city, Coordinates.Create(latitude, longitude));
        }
    }
}
