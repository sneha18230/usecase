
using Auction.Commom.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Auction.Commom.Repository
{
    public interface IProductRepository
    {
        Task<List<Product>> GetProducts();

        Task<Product> GetProduct(int Id);

        Task<Product> AddProduct(Product product);

        Task<Product> UpdateProduct(Product product);

        Task<bool> DeleteProduct(int productId);
    }
}
