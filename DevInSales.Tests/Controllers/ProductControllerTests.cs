
using DevInSales.Api.Controllers;
using DevInSales.Api.Dtos;
using DevInSales.Core.Data.Context;
using DevInSales.Core.Entities;
using DevInSales.Core.Services;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace DevInSales.Tests.Controllers
{
    public class ProductControllerTests
    {
        private ProductService _productService;
        public ProductControllerTests()
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
        public async Task HTTPGET_ObterProdutoPorId_ShouldReturnOk()
        {
            var controller = new ProductController(_productService);
            var result = await controller.ObterProdutoPorId(1);
            Assert.NotNull(result);
            result.ToString().Equals(HttpStatusCode.OK);
        }

        [Fact]
        public async Task HTTPGET_ObterProdutoPorId_ShouldReturnNotFound()
        {
            var controller = new ProductController(_productService);
            var result = await controller.ObterProdutoPorId(150);
            result.ToString().Equals(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task HTTPPUT_AtualizarProduto_ShouldReturnNoContent()
        {
            var controller = new ProductController(_productService);
            var result = await controller.AtualizarProduto(new AddProduct("Nome", 10m), 1);
            result.ToString().Equals(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task HTTPPUT_AtualizarProduto_ShouldReturnBadRequest()
        {
            var controller = new ProductController(_productService);
            var result = await controller.AtualizarProduto(new AddProduct("string", 10m), 1);
            result.ToString().Equals(HttpStatusCode.BadRequest);

            result = await controller.AtualizarProduto(new AddProduct("Produto 2", 10m), 1);
            result.ToString().Equals(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task HTTPDELETE_Delete_ShouldReturnNoContent()
        {
            var controller = new ProductController(_productService);
            var result = await controller.Delete(1);
            result.ToString().Equals(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task HTTPDELETE_Delete_ShouldReturnNotFound()
        {
            var controller = new ProductController(_productService);
            var result = await controller.Delete(150);
            result.ToString().Equals(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task HTTPGET_GetAll_ShouldReturnOK()
        {
            var controller = new ProductController(_productService);
            var result = await controller.GetAll("Produto 1", 1, 100);
            result.ToString().Equals(HttpStatusCode.OK);
        }

        [Fact]
        public async Task HTTPGET_GetAll_ShouldReturnBadRequest()
        {
            var controller = new ProductController(_productService);
            var result = await controller.GetAll("Produto 1", 100, 1);
            result.ToString().Equals(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task HTTPGET_GetAll_ShouldReturnNoContent()
        {
            var controller = new ProductController(_productService);
            var result = await controller.GetAll("Produto 10", 1, 100);
            result.ToString().Equals(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task HTTPPOST_PostProduct_ShouldReturnCreated()
        {
            var controller = new ProductController(_productService);
            var result = await controller.PostProduct(new AddProduct("Produto 20", 10m));
            result.ToString().Equals(HttpStatusCode.Created);
        }

        [Fact]
        public async Task HTTPPOST_PostProduct_ShouldReturnBadRequest()
        {
            var controller = new ProductController(_productService);
            var result = await controller.PostProduct(new AddProduct("Produto 2", 10m));
            result.ToString().Equals(HttpStatusCode.BadRequest);
        }
    }
}
