using DevInSales.Core.Data.Context;
using DevInSales.Core.Entities;
using DevInSales.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace DevInSales.Tests.Services
{
    public class StateServiceTest
    {
        private StateService _stateService;
        public StateServiceTest()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var context = new DataContext(options);
            _stateService = new StateService(context);
        }

        [Fact]
        public async Task GetById_ShouldReturnState()
        {
            var result = await _stateService.GetById(1);
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("Acre")]
        public async Task GetAll_ShouldReturnState(string? name)
        {
            var result = await _stateService.GetAll(name);
            Assert.NotEmpty(result);
            Assert.Contains("Acre", result.Select(x => x.Name));
        }
    }
}
