using DevInSales.Core.Data.Context;
using DevInSales.Core.Entities;
using DevInSales.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace DevInSales.Tests
{
    public class AddressServiceTest
    {
        private DataContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new DataContext(options);
        }

        [Fact]
        public void GetById_ShouldReturnAdreess()
        {
            using (var context = CreateContext())
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                var service = new AddressService(context);
                var result = service.GetById(1);
                Assert.NotNull(result);
                Assert.Equal(1, result.Id);
            }
        }

        [Fact]
        public void GetAll_ShouldReturnAdreess()
        {
            using (var context = CreateContext())
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                var service = new AddressService(context);
                var result = service.GetAll(1, 1, "Rua", "12345-678");
                Assert.NotNull(result);
            }
        }

        [Fact]
        public void Add_ShoulReturnAddress()
        {
            using (var context = CreateContext())
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                var service = new AddressService(context);
                var result = service.Add(new Address("Rua", "123456789", 10, "Complemento", 1));
                Assert.NotNull(result);
                Assert.Equal(1, result.Id);
            }
        }

        [Fact]
        public void Update_ShoulReturnAddress()
        {
            using (var context = CreateContext())
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                var service = new AddressService(context);
                var result = service.Update(new Address("Rua", "123456789", 10, "Complemento", 1));
                Assert.NotNull(result);
                Assert.Equal(1, result.Id);
            }
        }
    }
}
