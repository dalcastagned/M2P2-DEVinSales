using DevInSales.Core.Data.Context;
using DevInSales.Core.Entities;
using DevInSales.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace DevInSales.Tests.Services
{
    public class CityServiceTest
    {
        [Fact]
        public void GetById_ShouldReturnCity()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "GetById_ShouldReturnCity")
                .Options;

            using (var context = new DataContext(options))
            {
                var service = new CityService(context);
                var result = service.GetById(1);
                Assert.NotNull(result);
            }
        }

        [Fact]
        public void GetAll_ShouldReturnCity()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "GetAll_ShouldReturnCity")
                .Options;

            using (var context = new DataContext(options))
            {
                var service = new CityService(context);
                var result = service.GetAll(1, "");
                Assert.NotNull(result);
            }
        }

        [Fact]
        public void Add_ShouldReturnCity()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "Add_ShouldReturnCity")
                .Options;

            using (var context = new DataContext(options))
            {
                var service = new CityService(context);
                var result = service.Add(new City(1, "Cidade"));
                Assert.NotNull(result);
                Assert.Equal(1, result.Id);
            }
        }
    }
}
