using AspNetCore.Cqrs.Core.Abstractions.Entities;

namespace AspNetCore.Cqrs.Core.Locations.ReadModels
{
    public sealed class LocationReadModel : ReadModel
    {
        public string? Country { get; set; }
        public string? City { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
}
