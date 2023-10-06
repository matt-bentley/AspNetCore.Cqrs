using AspNetCore.Cqrs.Core.Locations.Entities;
using AspNetCore.Cqrs.Core.Locations.ReadModels;
using AspNetCore.Cqrs.Core.Locations.ValueObjects;

namespace AspNetCore.Cqrs.Core.Tests.Builders
{
    public class LocationBuilder
    {
        private string _country = "United Kingdom";
        private string _city = "London";
        private decimal _latitude = 51.51m;
        private decimal _longitude = -0.13m;

        public Location Build()
        {
            return Location.Create(_country, _city, Coordinates.Create(_latitude, _longitude));
        }

        public LocationReadModel BuildReadModel()
        {
            return new LocationReadModel()
            {
                Id = Guid.NewGuid(),
                City = _city,
                Latitude = _latitude,
                Longitude = _longitude,
                Country = _country
            };
        }

        public LocationBuilder WithCity(string city)
        {
            _city = city;
            return this;
        }

        public LocationBuilder WithLatitude(decimal latitude)
        {
            _latitude = latitude;
            return this;
        }
    }
}
