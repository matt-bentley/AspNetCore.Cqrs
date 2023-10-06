using AspNetCore.Cqrs.Application.Abstractions.Repositories;
using AspNetCore.Cqrs.Core.Abstractions.Entities;
using AspNetCore.Cqrs.Core.Tests.Builders;
using AspNetCore.Cqrs.Core.Tests.Factories;
using AspNetCore.Cqrs.Core.Weather.Entities;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Text.Json;
using System.Text;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Autofac;
using AspNetCore.Cqrs.Core.Locations.ReadModels;
using AspNetCore.Cqrs.Core.Weather.ReadModels;

namespace AspNetCore.Cqrs.Api.Tests
{
    internal class TestWebApplication : WebApplicationFactory<Program>
    {
        public readonly Mock<IRepository<WeatherForecast>> WeatherForecastsRepository;
        public readonly List<WeatherForecast> TestWeatherForecasts = new List<WeatherForecast>();

        public readonly Mock<IWeatherForecastsReadModelRepository> WeatherForecastsReadModelRepository;
        public readonly List<WeatherForecastReadModel> TestWeatherForecastReadModels = new List<WeatherForecastReadModel>();

        public readonly Mock<ILocationsReadModelRepository> LocationsRepository;
        public readonly List<LocationReadModel> TestLocations = new List<LocationReadModel>();

        public readonly Mock<IUnitOfWork> UnitOfWork = new Mock<IUnitOfWork>();

        private readonly List<(object Service, Type Type)> _replaceServices = new List<(object Service, Type Type)>();
        private readonly List<(object Service, Type Type)> _replaceAutofacServices = new List<(object Service, Type Type)>();

        public TestWebApplication()
        {
            WeatherForecastsRepository = CreateMockRepository(TestWeatherForecasts);
            TestLocations.Add(new LocationBuilder().BuildReadModel());

            WithAutofacReplacementService(UnitOfWork.Object);

            LocationsRepository = MockRepositoryFactory.CreateReadModel<LocationReadModel, ILocationsReadModelRepository>(TestLocations);
            WithAutofacReplacementService(LocationsRepository.Object);

            WeatherForecastsReadModelRepository = MockRepositoryFactory.CreateReadModel<WeatherForecastReadModel, IWeatherForecastsReadModelRepository>(TestWeatherForecastReadModels);
            WeatherForecastsReadModelRepository.Setup(e => e.GetByLocationAsync(It.IsAny<Guid>()))
                                               .Returns<Guid>(locationId => Task.FromResult(TestWeatherForecastReadModels.Where(e => e.LocationId == locationId).ToList()));
            WithAutofacReplacementService(WeatherForecastsReadModelRepository.Object);
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.ConfigureHostConfiguration(config =>
            {
                config.SetBasePath(Directory.GetCurrentDirectory())
                      .AddJsonFile("appsettings.test.json", false);
            });
            builder.UseEnvironment("Test");
            builder.ConfigureContainer<ContainerBuilder>(container =>
            {
                foreach(var service in _replaceAutofacServices)
                {
                    container.RegisterInstance(service.Service)
                        .As(service.Type);
                }
            });
            return base.CreateHost(builder);
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                foreach (var service in _replaceServices)
                {
                    ReplaceService(services, service.Service, service.Type);
                }
            });
            base.ConfigureWebHost(builder);
        }

        public TestWebApplication WithReplacementService<TService>(TService service) where TService : class
        {
            _replaceServices.Add((service, typeof(TService)));
            return this;
        }

        public TestWebApplication WithAutofacReplacementService<TService>(TService service) where TService : class
        {
            _replaceAutofacServices.Add((service, typeof(TService)));
            return this;
        }

        private void ReplaceService(IServiceCollection services, object service, Type serviceType)
        {
            var descriptor = services.FirstOrDefault(d => d.ServiceType.IsAssignableTo(serviceType));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }
            services.AddSingleton(serviceType, service);
        }

        public Mock<IRepository<T>> CreateMockRepository<T>(IEnumerable<T> items) where T : AggregateRoot
        {
            var repository = MockRepositoryFactory.Create(items);
            SetupRepository(repository.Object);
            return repository;
        }

        public void SetupRepository<T>(IRepository<T> repository) where T : AggregateRoot
        {
            _replaceServices.Add((repository, typeof(IRepository<T>)));
        }

        public StringContent GetStringContent<T>(T item)
        {
            return new StringContent(JsonSerializer.Serialize(item, new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }), Encoding.UTF8, "application/json");
        }

        public async Task<MultipartFormDataContent> GetTestFileFormAsync(string fileName = "example.mp4", string mediaContentType = "video/mp4", int sizeInMb = 1)
        {
            var form = new MultipartFormDataContent();
            var file = await GetTestFileAsync(sizeInMb);
            var fileContent = new ByteArrayContent(file, 0, file.Length);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(mediaContentType);
            form.Add(fileContent, "file", fileName);
            return form;
        }

        public async Task<byte[]> GetTestFileAsync(int sizeInMb)
        {
            const int blockSize = 1024 * 8;
            const int blocksPerMb = (1024 * 1024) / blockSize;
            byte[] data = new byte[blockSize];
            Random rng = new Random();
            var stream = new MemoryStream();
            for (int i = 0; i < sizeInMb * blocksPerMb; i++)
            {
                rng.NextBytes(data);
                await stream.WriteAsync(data, 0, data.Length);
            }
            return stream.ToArray();
        }
    }
}
