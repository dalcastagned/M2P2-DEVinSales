
using DevInSales.Api.Controllers;
using DevInSales.Api.Dtos;
using DevInSales.Core.Data.Context;
using DevInSales.Core.Entities;
using DevInSales.Core.Services;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace DevInSales.Tests.Controllers
{
    public class CityControllerTests
    {
        private CityService _cityService;
        private StateService _stateService;

        public CityControllerTests()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var context = new DataContext(options);
            _cityService = new CityService(context);
            _stateService = new StateService(context);
            Seed().Wait();
        }
        private async Task Seed()
        {
            await _cityService.Add(new City(1, "Cidade 1"));
            await _cityService.Add(new City(1, "Cidade 2"));
            await _cityService.Add(new City(2, "Cidade 3"));
        }

        [Fact]
        public async Task HTTPGET_GetCityByStateId_ShouldReturnOk()
        {
            var controller = new CityController(_stateService, _cityService);
            var result = await controller.GetCityByStateId(1, "");
            Assert.NotNull(result);
            result.ToString().Equals(HttpStatusCode.OK);
        }

        [Fact]
        public async Task HTTPGET_GetCityByStateId_ShouldReturnNotFound()
        {
            var controller = new CityController(_stateService, _cityService);
            var result = await controller.GetCityByStateId(150, "");
            Assert.NotNull(result);
            result.ToString().Equals(HttpStatusCode.NotFound);
        }
        
        [Fact]
        public async Task HTTPGET_GetCityByStateId_ShouldReturnNoContent()
        {
            var controller = new CityController(_stateService, _cityService);
            var result = await controller.GetCityByStateId(1, "Brusque");
            Assert.NotNull(result);
            result.ToString().Equals(HttpStatusCode.NoContent);
        }
        
        [Fact]
        public async Task HTTPGET_GetCityById_ShouldReturnOk()
        {
            var controller = new CityController(_stateService, _cityService);
            var result = await controller.GetCityById(1, 1);
            Assert.NotNull(result);
            result.ToString().Equals(HttpStatusCode.OK);
        }

        [Fact]
        public async Task HTTPGET_GetCityById_ShouldReturnBadRequest()
        {
            var controller = new CityController(_stateService, _cityService);
            var result = await controller.GetCityById(1, 3);
            Assert.NotNull(result);
            result.ToString().Equals(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task HTTPGET_GetCityById_ShouldReturnNotFound()
        {
            var controller = new CityController(_stateService, _cityService);
            var result = await controller.GetCityById(150, 1);
            Assert.NotNull(result);
            result.ToString().Equals(HttpStatusCode.NotFound);

            result = await controller.GetCityById(1, 150);
            Assert.NotNull(result);
            result.ToString().Equals(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task HTTPPOST_AddCity_ShouldReturnCreated()
        {
            var controller = new CityController(_stateService, _cityService);
            var result = await controller.AddCity(1, new AddCity("Cidade 4"));
            result.ToString().Equals(HttpStatusCode.Created);
        }

        [Fact]
        public async Task HTTPPOST_AddCity_ShouldReturnBadRequest()
        {
            var controller = new CityController(_stateService, _cityService);
            var result = await controller.AddCity(1, new AddCity("Cidade 1"));
            result.ToString().Equals(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task HTTPPOST_AddCity_ShouldReturnNotFound()
        {
            var controller = new CityController(_stateService, _cityService);
            var result = await controller.AddCity(150, new AddCity("Cidade 1"));
            result.ToString().Equals(HttpStatusCode.NotFound);
        }
    }
}
