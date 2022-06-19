using DevInSales.Core.Entities;

namespace DevInSales.Core.Interfaces
{
    public interface IDeliveryService
    {
        Task<List<Delivery>> GetBy(int? idAddress, int? saleId);
    }
}
