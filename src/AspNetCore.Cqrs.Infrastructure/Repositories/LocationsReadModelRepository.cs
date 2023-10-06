using AspNetCore.Cqrs.Application.Abstractions.Repositories;
using AspNetCore.Cqrs.Core.Locations.ReadModels;
using Dapper;

namespace AspNetCore.Cqrs.Infrastructure.Repositories
{
    public sealed class LocationsReadModelRepository : ILocationsReadModelRepository
    {
        private readonly DapperContext _context;

        public LocationsReadModelRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<List<LocationReadModel>> GetAllAsync()
        {
            var query = "SELECT * FROM Locations ORDER BY City";
            using var connection = _context.CreateConnection();
            var locations = await connection.QueryAsync<LocationReadModel>(query);
            return locations.ToList();
        }

        public async Task<LocationReadModel?> GetByIdAsync(Guid id)
        {
            var query = "SELECT TOP 1 * FROM Locations WHERE Id = @Id";
            using var connection = _context.CreateConnection();
            var location = await connection.QueryFirstOrDefaultAsync<LocationReadModel>(query, new { id });
            return location;
        }
    }
}
