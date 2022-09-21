using Auction.Commom.Model;
using Auction.Commom.Repository;
using Auction.Common.Exception;
using Auction.Common.Services;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace eAuction.UnitTest.Services
{
    [TestFixture]
    public class BuyerServiceTests
    {
        private MockRepository mockRepository;

        private Mock<ILogger<BuyerService>> mockLogger;
        private Mock<IProductRepository> mockProductRepository;

        [SetUp]
        public void SetUp()
        {
            this.mockRepository = new MockRepository(MockBehavior.Default);
            this.mockLogger = this.mockRepository.Create<ILogger<BuyerService>>();
            this.mockProductRepository = this.mockRepository.Create<IProductRepository>();
        }

        private BuyerService CreateService()
        {
            return new BuyerService(
                this.mockLogger.Object,
                this.mockProductRepository.Object);
        }

        [Test]
        public async Task AddBid_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
    
            Auction.Commom.Model.Buyer bid = new Auction.Commom.Model.Buyer
            {
                ProductId = 1,
                BidAmount = 100001,
                FirstName = "Sundhar",
                LastName = "Ravi",
                Address = "Kochi",
                City = "TVM",
                State = "KL",
                Pin = "700232",
                Phone = "987655321",
                Email = "PortKochiMehal@example.com"
            };

            Product product = getSampleProduct();

            mockProductRepository.Setup(pr => pr.GetProduct(It.IsAny<int>())).ReturnsAsync(product);
            
            mockProductRepository.Setup(pr => pr.UpdateProduct(It.IsAny<Product>())).ReturnsAsync(product);
            // Act
            var result = await service.AddBid(
                bid);

            // Assert
            Assert.AreEqual(result.Email, bid.Email);
        }

        [Test]
        public async Task AddBid_with_duplicateBid_throws_Buyer_Exception()
        {
            // Arrange
            var service = this.CreateService();

            Auction.Commom.Model.Buyer bid = new Auction.Commom.Model.Buyer
            {
                ProductId = 1,
                BidAmount = 100001,
                FirstName = "Sundhar",
                LastName = "Ravi",
                Address = "Kochi",
                City = "TVM",
                State = "KL",
                Pin = "700232",
                Phone = "987655321",
                Email = "PortKochiMehal@example.com"
            };

            Product product = getSampleProduct();

            product.Buyers.Add(bid);

            mockProductRepository.Setup(pr => pr.GetProduct(It.IsAny<int>())).ReturnsAsync(product);

            mockProductRepository.Setup(pr => pr.UpdateProduct(It.IsAny<Product>())).ReturnsAsync(product);
            // Act
            var ex =  Assert.ThrowsAsync<BuyerException>(() => service.AddBid(bid));

            // Assert
            Assert.AreEqual(ex.Message, "Buyer with Email already exists!");
        }

        [Test]
        public async Task AddBid_with_invalid_bidemail_throws_ValidationException()
        {
            // Arrange
            var service = this.CreateService();

            Auction.Commom.Model.Buyer bid = new Auction.Commom.Model.Buyer
            {
                ProductId = 1,
                BidAmount = 100001,
                FirstName = "Sundhar",
                LastName = "Ravi",
                Address = "Kochi",
                City = "TVM",
                State = "KL",
                Pin = "700232",
                Phone = "987655321",
                Email = "PortKochiMehal@example"
            };

            Product product = getSampleProduct();

            mockProductRepository.Setup(pr => pr.GetProduct(It.IsAny<int>())).ReturnsAsync(product);

            mockProductRepository.Setup(pr => pr.UpdateProduct(It.IsAny<Product>())).ReturnsAsync(product);
            // Act
            var ex = Assert.ThrowsAsync<ValidationException>(() => service.AddBid(bid));

            // Assert
            Assert.AreEqual(ex.Message, "Invlid Buyer's email!");
        }

        [Test]
        public async Task AddBid_with_invalid_bidamount_throws_ValidationException()
        {
            // Arrange
            var service = this.CreateService();

            Auction.Commom.Model.Buyer bid = new Auction.Commom.Model.Buyer
            {
                ProductId = 1,
                BidAmount = 0,
                FirstName = "Sundhar",
                LastName = "Ravi",
                Address = "Kochi",
                City = "TVM",
                State = "KL",
                Pin = "700232",
                Phone = "987655321",
                Email = "PortKochiMehal@example.com"
            };

            Product product = getSampleProduct();

            mockProductRepository.Setup(pr => pr.GetProduct(It.IsAny<int>())).ReturnsAsync(product);

            mockProductRepository.Setup(pr => pr.UpdateProduct(It.IsAny<Product>())).ReturnsAsync(product);
            // Act
            var ex = Assert.ThrowsAsync<ValidationException>(() => service.AddBid(bid));

            // Assert
            Assert.AreEqual(ex.Message, "Bid Amount should be greater than 1");
        }

        [Test]
        public async Task AddBid_post_biddate_throws_ValidationException()
        {
            // Arrange
            var service = this.CreateService();

            Auction.Commom.Model.Buyer bid = new Auction.Commom.Model.Buyer
            {
                ProductId = 1,
                BidAmount = 100001,
                FirstName = "Sundhar",
                LastName = "Ravi",
                Address = "Kochi",
                City = "TVM",
                State = "KL",
                Pin = "700232",
                Phone = "987655321",
                Email = "PortKochiMehal@example.com"
            };

            Product product = getSampleProduct();

            product.BidEndDate = DateTime.Now.AddDays(-1);

            mockProductRepository.Setup(pr => pr.GetProduct(It.IsAny<int>())).ReturnsAsync(product);

            mockProductRepository.Setup(pr => pr.UpdateProduct(It.IsAny<Product>())).ReturnsAsync(product);
            // Act
            var ex = Assert.ThrowsAsync<ValidationException>(() => service.AddBid(bid));

            // Assert
            Assert.AreEqual(ex.Message, "Bid can't be placed/modified past the Product's Bid end date, End date is : " + product.BidEndDate);
        }

        [Test]
        public async Task AddBid_with_lesserbidamount_thanExistingbids_throws_ValidationException()
        {
            // Arrange
            var service = this.CreateService();

            Auction.Commom.Model.Buyer bid = new Auction.Commom.Model.Buyer
            {
                ProductId = 1,
                BidAmount = 1000,
                FirstName = "Sundhar",
                LastName = "Ravi",
                Address = "Kochi",
                City = "TVM",
                State = "KL",
                Pin = "700232",
                Phone = "987655321",
                Email = "PortKochiMehal@example.com"
            };

            Product product = getSampleProduct();

            //product.Buyers.Add(bid);

            mockProductRepository.Setup(pr => pr.GetProduct(It.IsAny<int>())).ReturnsAsync(product);

            mockProductRepository.Setup(pr => pr.UpdateProduct(It.IsAny<Product>())).ReturnsAsync(product);
            // Act
            var ex = Assert.ThrowsAsync<ValidationException>(() => service.AddBid(bid));

            // Assert
            Assert.AreEqual(ex.Message, "Bid can't be lower than/equal to the Product's Starting Price, Product Price is : " + product.StartingPrice);
        }
        [Test]
        public async Task UpdateBid_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            int productId = 1;
            string email = "UnknownPortKochiMehal@example.com";
            double amount = 100001;

            Auction.Commom.Model.Buyer bid = new Auction.Commom.Model.Buyer
            {
                ProductId = 1,
                BidAmount = 200000,
                FirstName = "Sundhar",
                LastName = "Ravi",
                Address = "Kochi",
                City = "TVM",
                State = "KL",
                Pin = "700232",
                Phone = "987655321",
                Email = "PortKochiMehal@example.com"
            };

            Product product = getSampleProduct();

            product.Buyers.Add(bid);

            mockProductRepository.Setup(pr => pr.GetProduct(It.IsAny<int>())).ReturnsAsync(product);

            mockProductRepository.Setup(pr => pr.UpdateProduct(It.IsAny<Product>())).ReturnsAsync( product);
            // Act
            var ex = Assert.ThrowsAsync<BuyerException>(() => service.UpdateBid(productId,email,amount));

            // Assert
            Assert.AreEqual(ex.Message, "Bid not available for this Buyer!");


        }

        public static Product getSampleProduct()
        {
            Product product = new Product
            {
                ProductId = 1,
                ProductName = "Very Rare Platinum Necklace",
                ShortDescription = "Platinum Necklace",
                DetailedDescription = "18th Century Necklace worn by many mugal queens",
                Category = Categories.Ornament,
                StartingPrice = 100000,
                BidEndDate = DateTime.Today.AddDays(30),
                Seller = new Seller
                {
                    FirstName = "Guraham",
                    LastName = "Singh",
                    Address = "Mumbai",
                    City = "Mumbai",
                    State = "MH",
                    Pin = "400325",
                    Phone = "987654321",
                    Email = "SinghPalace@example.com"
                },
                Buyers = new System.Collections.Generic.List<Auction.Commom.Model.Buyer>()
            };

            return product;

        }
    }
}
