
using DevInSales.Api.Controllers;
using DevInSales.Core.Data.Context;
using DevInSales.Core.Services;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace DevInSales.Tests.Controllers
{
    public class StateControllerTests
    {
        private StateService _stateService;
        public StateControllerTests()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var context = new DataContext(options);
            _stateService = new StateService(context);
        }

        [Fact]
        public async Task HTTPGET_GetByStateId_ShouldReturnOk()
        {
            var controller = new StateController(_stateService);
            var result = await controller.GetByStateId(1);
            Assert.NotNull(result);
            result.ToString().Equals(HttpStatusCode.OK);
        }

        [Fact]
        public async Task HTTPGET_GetByStateId_ShouldReturnNotFound()
        {
            var controller = new StateController(_stateService);
            var result = await controller.GetByStateId(150);
            result.ToString().Equals(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task HTTPGET_GetAll_ShouldReturnOk()
        {
            var controller = new StateController(_stateService);
            var result = await controller.GetAll("Acre");
            result.ToString().Equals(HttpStatusCode.OK);
        }
        
        [Fact]
        public async Task HTTPGET_GetAll_ShouldReturnNoContent()
        {
            var controller = new StateController(_stateService);
            var result = await controller.GetAll("Brusque");
            result.ToString().Equals(HttpStatusCode.NoContent);
        }
    }
}
