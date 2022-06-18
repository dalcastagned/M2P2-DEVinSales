using DevInSales.Core.Entities;

namespace DevInSales.Core.Interfaces
{
    public interface IStateService
    {
        Task<IEnumerable<State>> GetAll(string? name);
        Task<State?> GetById(int stateId);
    }
}