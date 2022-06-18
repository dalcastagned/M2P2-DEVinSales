using DevInSales.Core.Entities;

namespace DevInSales.Core.Interfaces
{
    public interface IAddressService
    {
        Task<IEnumerable<Address>> GetAll(int? stateId, int? cityId, string? street, string? cep);
        Task<Address?> GetById(int addressId);
        Task Add(Address address);
        Task Update(Address address);
        Task Delete(Address address);
    }
}