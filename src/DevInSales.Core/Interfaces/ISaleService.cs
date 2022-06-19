using DevInSales.Core.Data.Dtos;
using DevInSales.Core.Entities;

namespace DevInSales.Core.Interfaces
{
    public interface ISaleService
    {
        Task<SaleResponse> GetSaleById(int id);

        Task<int> CreateSaleByUserId(Sale sale);

        Task<List<Sale>> GetSaleBySellerId(int? userId);

        Task<List<Sale>> GetSaleByBuyerId(int? userId);

        Task UpdateUnitPrice(int saleId, int productId, decimal price);

        Task UpdateAmount(int saleId, int productId, int amount);

        Task<int> CreateDeliveryForASale(Delivery delivery);

        Task<Delivery> GetDeliveryById(int deliveryId);
    }
}
