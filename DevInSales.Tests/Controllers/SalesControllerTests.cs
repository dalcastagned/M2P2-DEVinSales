using DevInSales.Api.Controllers;
using DevInSales.Api.Dtos;
using DevInSales.Core.Data.Context;
using DevInSales.Core.Data.Dtos;
using DevInSales.Core.Entities;
using DevInSales.Core.Services;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace DevInSales.Tests.Controllers
{
    public class SalesControllerTests
    {
        private SaleService _saleService;

        public SalesControllerTests()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var context = new DataContext(options);
            _saleService = new SaleService(context);
            Seed().Wait();
        }

        private async Task Seed()
        {
            await _saleService.CreateSaleByUserId(new Sale(1, 1, DateTime.Now));
        }

        [Fact]
        public async Task HTTPGET_ObterProdutoPorId_ShouldReturnOk()
        {
            var controller = new SalesController(_saleService);
            var result = await controller.GetSaleById(1);
            Assert.NotNull(result);
            result.ToString().Equals(HttpStatusCode.OK);
        }

        [Fact]
        public async Task HTTPGET_ObterProdutoPorId_ShouldReturnNotFound()
        {
            var controller = new SalesController(_saleService);
            var result = await controller.GetSaleById(150);
            Assert.NotNull(result);
            result.ToString().Equals(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task HTTPGET_GetSaleBySellerId_ShouldReturnOk()
        {
            var controller = new SalesController(_saleService);
            var result = await controller.GetSalesBySellerId(1);
            Assert.NotNull(result);
            result.ToString().Equals(HttpStatusCode.OK);
        }

        [Fact]
        public async Task HTTPGET_GetSaleBySellerId_ShouldReturnNotFound()
        {
            var controller = new SalesController(_saleService);
            var result = await controller.GetSalesBySellerId(150);
            Assert.NotNull(result);
            result.ToString().Equals(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task HTTPGET_GetSalesByBuyerId_ShouldReturnOk()
        {
            var controller = new SalesController(_saleService);
            var result = await controller.GetSalesByBuyerId(1);
            Assert.NotNull(result);
            result.ToString().Equals(HttpStatusCode.OK);
        }

        [Fact]
        public async Task HTTPGET_GetSalesByBuyerId_ShouldReturnNotFound()
        {
            var controller = new SalesController(_saleService);
            var result = await controller.GetSalesByBuyerId(150);
            Assert.NotNull(result);
            result.ToString().Equals(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task HTTPPOST_CreateSaleBySellerId_ShouldReturnCreated()
        {
            var controller = new SalesController(_saleService);
            var result = await controller.CreateSaleBySellerId(
                1,
                new SaleBySellerRequest(1, DateTime.Now)
            );
            result.ToString().Equals(HttpStatusCode.Created);
        }
    }
}
