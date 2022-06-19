using DevInSales.Core.Data.Dtos;
using DevInSales.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevInSales.Core.Interfaces
{
    public interface ISaleProductService
    {
        Task<int> GetSaleProductById(int id);

        Task<int> CreateSaleProduct(int saleId, SaleProductRequest saleProduct);
    }
}
