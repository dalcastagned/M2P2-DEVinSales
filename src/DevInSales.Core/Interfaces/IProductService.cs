using DevInSales.Core.Entities;

namespace DevInSales.Core.Interfaces
{
    public interface IProductService
    {
        Task Atualizar();
        Task<Product?> ObterProductPorId(int id);
        Task<bool> ProdutoExiste(string nome);
        Task Delete(int id);
        Task<List<Product>> ObterProdutos(string? name, decimal? priceMin, decimal? priceMax);
        Task<int> CreateNewProduct(Product product);
    }
}
