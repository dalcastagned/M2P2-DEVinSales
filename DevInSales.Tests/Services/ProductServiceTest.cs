using DevInSales.Core.Data.Context;
using DevInSales.Core.Entities;
using DevInSales.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace DevInSales.Tests.Services
{
    public class ProductServiceTest
    {
        private ProductService _productService;

        public ProductServiceTest()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var context = new DataContext(options);
            _productService = new ProductService(context);
            Seed().Wait();
        }

        private async Task Seed()
        {
            await _productService.CreateNewProduct(new Product("Produto 1", 10));
            await _productService.CreateNewProduct(new Product("Produto 2", 20));
            await _productService.CreateNewProduct(new Product("Produto 3", 30));
        }

        [Fact]
        public async Task GetById_ShouldReturnProduct()
        {
            var result = await _productService.ObterProductPorId(1);
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public async Task GetAll_ShouldReturnProduct()
        {
            var result = await _productService.ObterProdutos("Produto 1", 9m, 11m);
            Assert.NotEmpty(result);
            Assert.Contains("Produto 1", result.Select(x => x.Name));
            Assert.Contains(10, result.Select(x => x.SuggestedPrice));
        }

        [Fact]
        public async Task Update_ShoulReturnProduct()
        {
            var result = await _productService.ObterProductPorId(1);
            result.AtualizarDados("Novo Produto", 99);

            await _productService.Atualizar();

            var updateResult = await _productService.ObterProductPorId(1);

            Assert.Equal(99, result.SuggestedPrice);
        }

        [Fact]
        public async Task Delete_ShoulReturnNull()
        {
            var result = await _productService.ObterProductPorId(1);

            await _productService.Delete(result.Id);

            var deletedResult = await _productService.ObterProductPorId(1);

            Assert.Null(deletedResult);
        }
    }
}
