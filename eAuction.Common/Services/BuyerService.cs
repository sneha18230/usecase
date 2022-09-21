using Auction.Commom.Model;
using Auction.Commom.Repository;
using Auction.Common.Exception;
using Microsoft.Extensions.Logging;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Auction.Common.Services
{
    public class BuyerService : IBuyerService
    {
        private readonly ILogger<BuyerService> _logger;

        private readonly IProductRepository _repository;

        public BuyerService(ILogger<BuyerService> logger, IProductRepository repository)
        {
            this._logger = logger;

            this._repository = repository;
        }

        public async Task<Buyer> AddBid(Buyer bid)
        {
            _logger.LogInformation("Began" + nameof(AddBid), bid.Email);

            var product = await GetProduct(bid.ProductId).ConfigureAwait(false);

            if (ValidateBuyer(product, bid.Email))
            {
                throw new BuyerException("Buyer with Email already exists!");
            }

            ValidateBidForProduct(product, bid.Email, bid.BidAmount);

            product.Buyers.Add(bid);

            var response = await _repository.UpdateProduct(product).ConfigureAwait(false);

            _logger.LogInformation("Ended" + nameof(AddBid));

            var buyer = response?.Buyers.Find(m => m.Email.Equals(bid.Email, StringComparison.InvariantCultureIgnoreCase)
            && m.ProductId == product.ProductId);

            return buyer ?? new Buyer();
        }

        public async Task<Buyer> UpdateBid(int productId, string email, double amount)
        {
            _logger.LogInformation("Began" + nameof(UpdateBid), productId);

            var product = await GetProduct(productId).ConfigureAwait(false);

            ValidateBidForProduct(product, email, amount);

            if (!ValidateBuyer(product, email))
            {
                throw new BuyerException("Bid not available for this Buyer!");
            }

            product.Buyers.ForEach(b =>
            {
                if (b.Email.Equals(email, StringComparison.InvariantCultureIgnoreCase))
                {
                    b.BidAmount = amount;
                }
            });
            var response = await _repository.UpdateProduct(product).ConfigureAwait(false);

            _logger.LogInformation("Ended" + nameof(UpdateBid));

            var buyer = response?.Buyers.Find(m => m.Email.Equals(email, StringComparison.InvariantCultureIgnoreCase) && m.ProductId == productId);

            return buyer ?? new Buyer();
        }

        private async Task<Product> GetProduct(int Id)
        {
            var product = new Product();

            product = await _repository.GetProduct(Id).ConfigureAwait(false);

            if (product?.ProductId == null)
            {
                throw new ProductException("Product not available");
            }

            return product;
        }

        private bool ValidateBuyer(Product product, string email)
        {
            return product.Buyers.FindAll(x => x.Email.Equals(email, StringComparison.InvariantCultureIgnoreCase)).Count > 0;

        }

        private void ValidateBidForProduct(Product product, string email, double amount)
        {
            if (!IsValidEmail(email))
            {
                throw new ValidationException("Invlid Buyer's email!");
            }
            if (amount < 1)
            {
                throw new ValidationException("Bid Amount should be greater than 1");
            }
            if (product.BidEndDate < DateTime.Now)
            {
                throw new ValidationException("Bid can't be placed/modified past the Product's Bid end date, End date is : " + product.BidEndDate);
            }
            if (product.StartingPrice >= amount)
            {
                throw new ValidationException("Bid can't be lower than/equal to the Product's Starting Price, Product Price is : " + product.StartingPrice);
            }
        }

        private bool IsValidEmail(string email)
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");

            Match match = regex.Match(email);

            return match.Success;
        }

    }
}