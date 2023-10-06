using AspNetCore.Cqrs.Core.Locations.ReadModels;

namespace AspNetCore.Cqrs.Api.Tests.Controllers
{
    public class LocationsControllerTests
    {
        private const string BASE_URL = "api/locations";
        private readonly TestWebApplication _application = new();

        [Fact]
        public async Task GivenLocationsController_WhenGetById_ThenOk()
        {
            using var client = _application.CreateClient();
            var response = await client.GetAsync($"{BASE_URL}/{_application.TestLocations.First().Id}");

            var locations = await response.ReadAndAssertSuccessAsync<LocationReadModel>();

            locations.Should().NotBeNull();
        }

        [Fact]
        public async Task GivenLocationsController_WhenGet_ThenOk()
        {
            using var client = _application.CreateClient();
            var response = await client.GetAsync(BASE_URL);

            var locations = await response.ReadAndAssertSuccessAsync<List<LocationReadModel>>();

            locations.Should().HaveCount(_application.TestLocations.Count);
        }
    }
}
