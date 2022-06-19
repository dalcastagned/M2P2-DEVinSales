using DevInSales.Core.Data.Context;
using DevInSales.Core.Entities;
using DevInSales.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DevInSales.Core.Services
{
    public class ProductService : IProductService
    {
        private readonly DataContext _context;

        public ProductService(DataContext context)
        {
            _context = context;
        }

        public async Task Atualizar()
        {
            await _context.SaveChangesAsync();
        }

        // obtém o produto por id
        public async Task<Product?> ObterProductPorId(int id)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        }

        // verifica se o nome já existe na base de dados
        public async Task<bool> ProdutoExiste(string nome)
        {
            var produtos = await _context.Products
                .Where(produto => (produto.Name.ToUpper() == nome.ToUpper()))
                .ToListAsync();
            return produtos.Count > 0 ? true : false;
        }

        public async Task Delete(int id)
        {
            var produto = await ObterProductPorId(id);
            if (produto == null)
                throw new Exception("o Produto não existe");
            _context.Products.Remove(produto);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Product>> ObterProdutos(
            string? name,
            decimal? priceMin,
            decimal? priceMax
        )
        {
            var query = _context.Products.AsQueryable();
            if (!string.IsNullOrEmpty(name))
                query = query.Where(p => p.Name.ToUpper().Contains(name.ToUpper()));
            if (priceMin.HasValue)
                query = query.Where(p => p.SuggestedPrice >= priceMin);
            if (priceMax.HasValue)
                query = query.Where(p => p.SuggestedPrice <= priceMax);

            return await query.ToListAsync();
        }

        public async Task<int> CreateNewProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product.Id;
        }
    }
}
