using Auction.Commom.Context;
using Auction.Commom.Model;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Auction.Commom.Repository
{
    public class ProductRepository : IProductRepository
    {

        private readonly IAuctionMongoContext auctionMongoContext;

        public ProductRepository(IAuctionMongoContext _auctionMongoContext)
        {
            auctionMongoContext = _auctionMongoContext;
        }

        public async Task<Product> AddProduct(Product product)
        {
            try
            {
                await auctionMongoContext.products.InsertOneAsync(product).ConfigureAwait(false);
                return product;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> DeleteProduct(int productId)
        {
            var filterDefinition = Builders<Product>.Filter.Eq(x => x.ProductId, productId);
            var result= await auctionMongoContext.products.DeleteOneAsync(filterDefinition);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        public Task<Product> GetProduct(int Id)
        {
            return auctionMongoContext.products.FindAsync(x => x.ProductId == Id).Result.FirstOrDefaultAsync();
        }

        public Task<List<Product>> GetProducts()
        {
            return auctionMongoContext.products.Find(_ => true).ToListAsync();
        }

        public Task<Product> UpdateProduct(Product product)
        {
            var filterDefinition = Builders<Product>.Filter.Eq(x => x.ProductId, product.ProductId);
            return auctionMongoContext.products.FindOneAndReplaceAsync(filterDefinition, product,
                new FindOneAndReplaceOptions<Product>()
                {
                    ReturnDocument = ReturnDocument.After,

                });
        }

        public Task<Product> AddBidToProduct(Buyer buyer)
        {
            var filterDefinition = Builders<Product>.Filter.Eq(x => x.ProductId, buyer.ProductId);
            var updateDefinition = Builders<Product>.Update.AddToSet(x => x.Buyers, buyer);
            return auctionMongoContext.products.FindOneAndUpdateAsync(filterDefinition, updateDefinition);
        }
    }
}
