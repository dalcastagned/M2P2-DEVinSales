using DevInSales.Core.Data.Context;
using DevInSales.Core.Data.Dtos;
using DevInSales.Core.Entities;
using DevInSales.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DevInSales.Core.Services
{
    public class SaleProductService : ISaleProductService
    {
        private readonly DataContext _context;

        public SaleProductService(DataContext context)
        {
            _context = context;
        }

        public async Task<int> CreateSaleProduct(int saleId, SaleProductRequest saleProduct)
        {
            if (
                !await _context.Products.AnyAsync(p => p.Id == saleProduct.ProductId)
                || !await _context.Sales.AnyAsync(p => p.Id == saleId)
            )
                throw new ArgumentException("ProductId ou SaleId não encontrado.");

            if (saleProduct.UnitPrice == null)
            {
                var product = await _context.Products.FirstOrDefaultAsync(
                    p => p.Id == saleProduct.ProductId
                );
                saleProduct.UnitPrice = product?.SuggestedPrice;
            }

            if (saleProduct.UnitPrice <= 0 || saleProduct.Amount <= 0)
                throw new ArgumentException("Preço ou quantidade não podem ser negativos.");

            var saleProductEntity = saleProduct.ConvertIntoSaleProduct(saleId);
            _context.SaleProducts.Add(saleProductEntity);
            await _context.SaveChangesAsync();

            return saleProductEntity.Id;
        }

        public async Task<int> GetSaleProductById(int saleProductId)
        {
            SaleProduct? saleProduct = await _context.SaleProducts
                .Include(p => p.Sales)
                .Include(p => p.Products)
                .FirstOrDefaultAsync(p => p.Id == saleProductId);

            if (saleProduct == null)
            {
                return 0;
            }

            return saleProduct.Id;
        }
    }
}
