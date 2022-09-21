using Auction.Commom.Model;
using Auction.Commom.Repository;
using Auction.Common.Exception;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auction.Common.Services
{
    public class SellerService : ISellerService
    {
        private readonly ILogger<SellerService> _logger;

        private readonly IProductRepository _repository;

        public SellerService(ILogger<SellerService> logger, IProductRepository repository)
        {
            this._logger = logger;

            this._repository = repository;
        }

        public async Task<Product> AddProduct(Product product)
        {
            this.ValidateAddProduct(product);

            return _repository.AddProduct(product).Result;

        }

        public async Task<bool> DeleteProduct(int productId)
        {
            var product = await this.GetProduct(productId, true).ConfigureAwait(false);

            this.VaidateDeleteProduct(product);

            return await _repository.DeleteProduct(productId).ConfigureAwait(false);
        }

        public async Task<Product> GetProductBids(int productId)
        {
            var product = await this.GetProduct(productId, true).ConfigureAwait(false);

            return product;
        }

        public async Task<List<Product>> GetProducts()
        {
            var products = await _repository.GetProducts().ConfigureAwait(false);


            products?.ForEach(p =>
            {
                p.Buyers = p.Buyers.OrderBy(x => x.BidAmount).ToList();
            });

            _logger.LogInformation("Ended" + nameof(GetProducts));

            return products;
        }

        private async Task<Product> GetProduct(int id, bool isProductExpected)
        {
            var product = new Product();

            product = await _repository.GetProduct(id).ConfigureAwait(false);

            if (product != null)
                product?.Buyers.OrderBy(x => x.BidAmount).ToList();

            return product;
        }


        private void ValidateAddProduct(Product product)
        {
            if (product.Buyers != null && product.Buyers?.Count > 0)
            {
                throw new ValidationException("Bids can't be added while creating a new Product!");
            }
        }

        private void VaidateDeleteProduct(Product product)
        {
            if (product.Buyers?.Count > 0)
            {
                throw new ValidationException("Product with a bid cannot be deleted");
            }

            if (product.BidEndDate <= DateTime.Now)
            {
                throw new ValidationException("Product with completed bid date cannot be delted");
            }
        }
    }
}