using DevInSales.Core.Data.Context;
using DevInSales.Core.Entities;
using DevInSales.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace DevInSales.Tests.Services
{
    public class CityServiceTest
    {
        private CityService _cityService;

        public CityServiceTest()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var context = new DataContext(options);
            _cityService = new CityService(context);
            Seed().Wait();
        }

        private async Task Seed()
        {
            await _cityService.Add(new City(1, "Cidade 1"));
            await _cityService.Add(new City(1, "Cidade 2"));
            await _cityService.Add(new City(1, "Cidade 3"));
        }

        [Fact]
        public async Task GetById_ShouldReturnCity()
        {
            var result = await _cityService.GetById(1);
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        [Theory]
        [InlineData(1, null)]
        [InlineData(1, "Cidade 1")]
        public async Task GetAll_ShouldReturnCity(int stateId, string? name)
        {
            var result = await _cityService.GetAll(stateId, name);
            Assert.NotEmpty(result);
            Assert.Contains(1, result.Select(x => x.StateId));
            Assert.Contains("Cidade 1", result.Select(x => x.Name));
        }

        [Fact]
        public async Task Add_ShouldReturnCity()
        {
            await _cityService.Add(new City(1, "Cidade 4"));

            var result = await _cityService.GetAll(1, "Cidade 4");
            Assert.Contains("Cidade 4", result.Select(x => x.Name));
        }
    }
}
