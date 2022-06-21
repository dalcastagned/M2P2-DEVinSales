using DevInSales.Api.Controllers;
using DevInSales.Api.Dtos;
using DevInSales.Core.Data.Context;
using DevInSales.Core.Entities;
using DevInSales.Core.Services;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace DevInSales.Tests.Controllers
{
    public class AddressControllerTests
    {
        private CityService _cityService;
        private StateService _stateService;
        private AddressService _addressService;

        public AddressControllerTests()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var context = new DataContext(options);
            _cityService = new CityService(context);
            _stateService = new StateService(context);
            _addressService = new AddressService(context);
            Seed().Wait();
        }

        private async Task Seed()
        {
            await _cityService.Add(new City(1, "Cidade 1"));
            await _cityService.Add(new City(1, "Cidade 2"));
            await _cityService.Add(new City(2, "Cidade 3"));
            await _addressService.Add(new Address("Rua 1", "123456789", 8, "Complemento 1", 1));
            await _addressService.Add(new Address("Rua 2", "123456788", 9, "Complemento 2", 1));
            await _addressService.Add(new Address("Rua 3", "123456787", 10, "Complemento 3", 1));
        }

        [Theory]
        [InlineData(1, null, null, null)]
        [InlineData(null, 1, null, null)]
        [InlineData(null, null, "Rua 1", null)]
        [InlineData(null, null, null, "123456789")]
        public async Task HTTPGET_GetAll_ShouldReturnOk(
            int? stateId,
            int? cityId,
            string? street,
            string? cep
        )
        {
            var controller = new AddressController(_addressService, _stateService, _cityService);
            var result = await controller.GetAll(stateId, cityId, street, cep);
            Assert.NotNull(result);
            result.ToString().Equals(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData(150, null, null, null)]
        [InlineData(null, 150, null, null)]
        [InlineData(null, null, "Rua 10", null)]
        [InlineData(null, null, null, "987654321")]
        public async Task HTTPGET_GetAll_ShouldReturnNoContent(
            int? stateId,
            int? cityId,
            string? street,
            string? cep
        )
        {
            var controller = new AddressController(_addressService, _stateService, _cityService);
            var result = await controller.GetAll(stateId, cityId, street, cep);
            Assert.NotNull(result);
            result.ToString().Equals(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task HTTPPOST_AddAddress_ShouldReturnCreated()
        {
            var controller = new AddressController(_addressService, _stateService, _cityService);
            var result = await controller.AddAddress(
                1,
                1,
                new AddAddress("Rua 4", 15, "Complemento 3", "123456786")
            );
            Assert.NotNull(result);
            result.ToString().Equals(HttpStatusCode.Created);
        }

        [Fact]
        public async Task HTTPPOST_AddAddress_ShouldReturnBadRequest()
        {
            var controller = new AddressController(_addressService, _stateService, _cityService);
            var result = await controller.AddAddress(
                1,
                3,
                new AddAddress("Rua 4", 15, "Complemento 3", "123456786")
            );
            Assert.NotNull(result);
            result.ToString().Equals(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task HTTPPOST_AddAddress_ShouldReturnNotFound()
        {
            var controller = new AddressController(_addressService, _stateService, _cityService);
            var result = await controller.AddAddress(
                150,
                1,
                new AddAddress("Rua 4", 15, "Complemento 3", "123456786")
            );
            result.ToString().Equals(HttpStatusCode.NotFound);

            result = await controller.AddAddress(
                1,
                150,
                new AddAddress("Rua 4", 15, "Complemento 3", "123456786")
            );
            result.ToString().Equals(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task HTTPDELETE_DeleteAddress_ShouldReturnNoContent()
        {
            var controller = new AddressController(_addressService, _stateService, _cityService);
            var result = await controller.DeleteAddress(1);
            result.ToString().Equals(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task HTTPDELETE_DeleteAddress_ShouldReturnNotFound()
        {
            var controller = new AddressController(_addressService, _stateService, _cityService);
            var result = await controller.DeleteAddress(150);
            result.ToString().Equals(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task HTTPPATCH_UpdateAddress_ShouldReturnNoContent()
        {
            var controller = new AddressController(_addressService, _stateService, _cityService);
            var result = await controller.UpdateAddress(1, new UpdateAddress("Rua 5", 15, "Complemento 3", "123456786"));
            result.ToString().Equals(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task HTTPPATCH_UpdateAddress_ShouldReturnBadRequest()
        {
            var controller = new AddressController(_addressService, _stateService, _cityService);
            var result = await controller.UpdateAddress(1, new UpdateAddress(null, null, null, null));
            result.ToString().Equals(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task HTTPPATCH_UpdateAddress_ShouldReturnNotFound()
        {
            var controller = new AddressController(_addressService, _stateService, _cityService);
            var result = await controller.UpdateAddress(150, new UpdateAddress("Rua 5", 15, "Complemento 3", "123456786"));
            result.ToString().Equals(HttpStatusCode.NotFound);
        }        
    }
}
