using DevInSales.Core.Entities;

namespace DevInSales.Core.Interfaces
{
    public interface ICityService
    {
        Task<IEnumerable<City>> GetAll(int stateId, string? name);
        Task<City?> GetById(int cityId);
        Task Add(City city);
    }
}