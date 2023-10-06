using AspNetCore.Cqrs.Api.Infrastructure.ActionResults;
using AspNetCore.Cqrs.Application.Weather.Models;
using AspNetCore.Cqrs.Core.Tests.Builders;
using AspNetCore.Cqrs.Core.Weather.Entities;
using AspNetCore.Cqrs.Core.Weather.ReadModels;

namespace AspNetCore.Cqrs.Api.Tests.Controllers
{
    public class WeatherForecastsControllerTests
    {
        private const string BASE_URL = "api/weatherforecasts";
        private readonly TestWebApplication _application = new();

        public WeatherForecastsControllerTests()
        {
            AddTestWeatherForecast();
            AddTestWeatherForecast();
            AddTestWeatherForecast();
        }

        private void AddTestWeatherForecast()
        {
            var builder = new WeatherForecastBuilder();
            _application.TestWeatherForecasts.Add(builder.Build());
            _application.TestWeatherForecastReadModels.Add(builder.BuildReadModel());
        }

        [Fact]
        public async Task GivenWeatherForecastsController_WhenGet_ThenOk()
        {
            using var client = _application.CreateClient();
            var response = await client.GetAsync(BASE_URL);

            var forecast = await response.ReadAndAssertSuccessAsync<List<WeatherForecastReadModel>>();

            forecast.Should().HaveCount(_application.TestWeatherForecastReadModels.Count);
        }

        [Fact]
        public async Task GivenWeatherForecastsController_WhenGetByLocation_ThenOk()
        {
            var locationId = Guid.NewGuid();
            _application.TestWeatherForecastReadModels.Add(new WeatherForecastBuilder().WithLocation(locationId).BuildReadModel());
            using var client = _application.CreateClient();
            var response = await client.GetAsync($"{BASE_URL}?locationid={locationId}");

            var forecast = await response.ReadAndAssertSuccessAsync<List<WeatherForecastReadModel>>();

            forecast.Should().HaveCount(1);
        }

        [Fact]
        public async Task GivenWeatherForecastsController_WhenGetById_ThenOk()
        {
            using var client = _application.CreateClient();
            var response = await client.GetAsync($"{BASE_URL}/{_application.TestWeatherForecastReadModels.First().Id}");

            var forecast = await response.ReadAndAssertSuccessAsync<WeatherForecastReadModel>();

            forecast!.Id.Should().Be(_application.TestWeatherForecastReadModels.First().Id);
        }

        [Fact]
        public async Task GivenWeatherForecastsController_WhenGetByMissingId_ThenNotFound()
        {
            using var client = _application.CreateClient();
            var id = Guid.NewGuid();
            var response = await client.GetAsync($"{BASE_URL}/{id}");

            var error = await response.ReadAndAssertError(HttpStatusCode.NotFound);
            error.Status.Should().Be(404);
        }

        [Fact]
        public async Task GivenWeatherForecastsController_WhenUpdate_ThenNoContent()
        {
            using var client = _application.CreateClient();
            var newDate = DateTime.UtcNow.AddDays(-1);
            var forecast = _application.TestWeatherForecasts.First();
            var response = await client.PutAsync($"{BASE_URL}/{forecast.Id}", _application.GetStringContent(new WeatherForecastUpdateDto() { Date = newDate }));

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            forecast.Date.Should().Be(newDate);
            _application.UnitOfWork.Verify(e => e.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GivenWeatherForecastsController_WhenCreate_ThenOk()
        {
            using var client = _application.CreateClient();
            var summary = "Mild";
            var date = DateTime.UtcNow;
            var response = await client.PostAsync(BASE_URL, _application.GetStringContent(new WeatherForecastCreateDto() { Date = date, Summary = summary, TemperatureC = 20, LocationId = _application.TestLocations.First().Id }));

            var result = await response.ReadAndAssertSuccessAsync<CreatedResultEnvelope>();
            result!.Id.Should().NotBeEmpty();
            _application.UnitOfWork.Verify(e => e.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
            _application.WeatherForecastsRepository.Verify(e => e.Insert(It.Is<WeatherForecast>(e => e.Summary == summary && e.Date == date)), Times.Once);
        }

        [Fact]
        public async Task GivenWeatherForecastsController_WhenCreateInvalid_ThenBadRequest()
        {
            using var client = _application.CreateClient();
            var response = await client.PostAsync(BASE_URL, _application.GetStringContent(new WeatherForecastCreateDto() { Summary = "", TemperatureC = 20, Date = DateTime.UtcNow, LocationId = _application.TestLocations.First().Id }));

            var error = await response.ReadAndAssertError(HttpStatusCode.BadRequest);
            error.ErrorMessage.Should().Be("Required input 'Summary' is missing.");
            _application.UnitOfWork.Verify(e => e.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task GivenWeatherForecastsController_WhenCreateInvalidLocation_ThenNotFound()
        {
            using var client = _application.CreateClient();
            var locationId = Guid.NewGuid();
            var response = await client.PostAsync(BASE_URL, _application.GetStringContent(new WeatherForecastCreateDto() { Summary = "", TemperatureC = 20, Date = DateTime.UtcNow, LocationId = locationId }));

            var error = await response.ReadAndAssertError(HttpStatusCode.NotFound);
            error.ErrorMessage.Should().Be($"Location not found: {locationId}");
            _application.UnitOfWork.Verify(e => e.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task GivenWeatherForecastsController_WhenDelete_ThenNoContent()
        {
            using var client = _application.CreateClient();
            var id = _application.TestWeatherForecasts.First().Id;
            var response = await client.DeleteAsync($"{BASE_URL}/{id}");
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            _application.WeatherForecastsRepository.Verify(e => e.Delete(It.Is<WeatherForecast>(e => e.Id == id)), Times.Once);
            _application.UnitOfWork.Verify(e => e.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
