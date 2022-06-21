using DevInSales.Core.Data.Context;
using DevInSales.Core.Entities;
using DevInSales.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace DevInSales.Tests.Services
{
    public class SaleServiceTest
    {
        private SaleService _saleService;
        public SaleServiceTest()
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
        public async Task GetSaleById_ShouldReturnSale()
        {
            var result = await _saleService.GetSaleById(10);
            Assert.Null(result);
            
            result = await _saleService.GetSaleById(1);
            Assert.NotNull(result);
            Assert.Equal(1, result.SaleId);
        }

        [Fact]
        public async Task GetSaleBySellerId_ShouldReturnSale()
        {
            var result = await _saleService.GetSaleBySellerId(1);
            Assert.NotEmpty(result);
            Assert.Contains(1, result.Select(x => x.Id));
        }


        [Fact]
        public async Task GetSaleByBuyerId_ShouldReturnSale()
        {
            var result = await _saleService.GetSaleByBuyerId(1);
            Assert.NotEmpty(result);
            Assert.Contains(1, result.Select(x => x.Id));
        }
    }
}
