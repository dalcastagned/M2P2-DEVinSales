using DevInSales.Core.Data.Context;
using DevInSales.Core.Data.Dtos;
using DevInSales.Core.Entities;

using DevInSales.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DevInSales.Core.Services
{
    public class SaleService : ISaleService
    {
        private readonly DataContext _context;

        public SaleService(DataContext context)
        {
            _context = context;
        }

        public async Task<int> CreateSaleByUserId(Sale sale)
        {
            if (sale.SaleDate == DateTime.MinValue)
                sale.SetSaleDateToToday();
            if (sale.BuyerId == 0 || sale.SellerId == 0)
                throw new ArgumentNullException("Id não pode ser nulo nem zero.");
            if (!await _context.Users.AnyAsync(user => user.Id == sale.BuyerId))
                throw new ArgumentException("BuyerId não encontrado.");
            if (!await _context.Users.AnyAsync(user => user.Id == sale.SellerId))
                throw new ArgumentException("SellerId não encontrado.");

            _context.Sales.Add(sale);
            await _context.SaveChangesAsync();

            return sale.Id;
        }

        public async Task<SaleResponse> GetSaleById(int id)
        {
            Sale? sale = await _context.Sales
                .Include(p => p.Buyer)
                .Include(p => p.Seller)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (sale == null)
            {
                return null;
            }

            var listaProdutos = await GetSaleProductsBySaleId(id);

            return new SaleResponse(
                sale.Id,
                sale.Seller.Name,
                sale.Buyer.Name,
                sale.SaleDate,
                listaProdutos
            );
        }

        public async Task<List<SaleProductResponse>> GetSaleProductsBySaleId(int id)
        {
            return await _context.SaleProducts
                .Where(p => p.SaleId == id)
                .Include(p => p.Products)
                .Select(
                    p =>
                        new SaleProductResponse(
                            p.Products.Name,
                            p.Amount,
                            p.UnitPrice,
                            p.Amount * p.UnitPrice
                        )
                )
                .ToListAsync();
        }

        public async Task<List<Sale>> GetSaleBySellerId(int? userId)
        {
            return await _context.Sales.Where(p => p.SellerId == userId).ToListAsync();
        }

        public async Task<List<Sale>> GetSaleByBuyerId(int? userId)
        {
            return await _context.Sales.Where(p => p.BuyerId == userId).ToListAsync();
        }

        public async Task UpdateUnitPrice(int saleId, int productId, decimal price)
        {
            Sale? sale = await _context.Sales.FirstOrDefaultAsync(p => p.Id == saleId);
            if (sale == null)
                throw new Exception();

            SaleProduct? saleproduct = await _context.SaleProducts.FirstOrDefaultAsync(
                p => p.ProductId == productId
            );

            if (saleproduct == null)
                throw new Exception();

            if (price <= 0)
                throw new ArgumentException();

            saleproduct.UpdateUnitPrice(price);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAmount(int saleId, int productId, int amount)
        {
            if (!await _context.Sales.AnyAsync(p => p.Id == saleId))
                throw new ArgumentException("Não existe venda com esse Id.", "saleId");

            var saleProduct = await _context.SaleProducts.FirstOrDefaultAsync(
                p => p.ProductId == productId
            );

            if (saleProduct == null)
                throw new ArgumentException("Não existe este produto nesta venda.", "productId");

            if (amount <= 0)
                throw new ArgumentException(
                    "Quantidade não pode ser menor ou igual a zero.",
                    "amount"
                );

            saleProduct.UpdateAmount(amount);

            await _context.SaveChangesAsync();
        }

        public async Task<int> CreateDeliveryForASale(Delivery delivery)
        {
            Sale? sale = await _context.Sales.FirstOrDefaultAsync(p => p.Id == delivery.SaleId);

            if (sale == null)
                throw new ArgumentException("Não existe venda com esse Id.", "saleId");

            Address? address = await _context.Addresses.FirstOrDefaultAsync(
                p => p.Id == delivery.AddressId
            );

            if (address == null)
                throw new ArgumentException("Não existe endereço com esse Id.", "AddressId");

            _context.Deliveries.Add(delivery);
            await _context.SaveChangesAsync();

            return delivery.Id;
        }

        public async Task<Delivery?> GetDeliveryById(int deliveryId)
        {
            return await _context.Deliveries.FirstOrDefaultAsync(p => p.Id == deliveryId);
        }
    }
}
