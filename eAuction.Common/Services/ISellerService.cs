using Auction.Commom.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Auction.Common.Services
{
    public interface ISellerService
    {
        Task<Product> AddProduct(Product product);

        Task<bool> DeleteProduct(int productId);

        Task<Product> GetProductBids(int productId);

        Task<List<Product>> GetProducts();
    }
}