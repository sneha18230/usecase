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
    public class SellerServiceTests
    {
        private MockRepository mockRepository;

        private Mock<ILogger<SellerService>> mockLogger;
        private Mock<IProductRepository> mockProductRepository;

        [SetUp]
        public void SetUp()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockLogger = this.mockRepository.Create<ILogger<SellerService>>();
            this.mockProductRepository = this.mockRepository.Create<IProductRepository>();
        }

        private SellerService CreateService()
        {
            return new SellerService(
                this.mockLogger.Object,
                this.mockProductRepository.Object);
        }

        [Test]
        public async Task AddProduct_Successfully_adds_Product()
        {
            // Arrange
            var service = this.CreateService();
            
            Product product = new Product { 
            ProductId=1 ,
            ProductName = "Very Rare Platinum Necklace",
            ShortDescription= "Platinum Necklace",
            DetailedDescription= "18th Century Necklace worn by many mugal queens",
            Category= Categories.Ornament,
            StartingPrice= 100000,
            BidEndDate= new DateTime(),
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
            Buyers = { }
            };
            
            this.mockProductRepository.Setup(x => x.AddProduct(It.IsAny<Product>())).ReturnsAsync(product);
            
            // Act
            var result = await service.AddProduct(
                product);

            // Assert
            Assert.AreEqual(result.ProductId,1, "The product is not matching");
        }

        [Test]
        public async Task AddProduct_WithBid_ThrowsException()
        {
            // Arrange
            var service = this.CreateService();

            Product product = new Product
            {
                ProductId = 1,
                ProductName = "Very Rare Platinum Necklace",
                ShortDescription = "Platinum Necklace",
                DetailedDescription = "18th Century Necklace worn by many mugal queens",
                Category = Categories.Ornament,
                StartingPrice = 100000,
                BidEndDate = new DateTime(),
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
                {
                    new  Auction.Commom.Model.Buyer
                    {
                        ProductId= 1,
                        BidAmount= 100001,
                        FirstName= "Sundhar",
                        LastName= "Ravi",
                        Address= "Kochi",
                        City= "TVM",
                        State= "KL",
                        Pin= "700232",
                        Phone= "987655321",
                        Email= "PortKochiMehal@example.com"
                    }
                }
            };

            this.mockProductRepository.Setup(x => x.AddProduct(It.IsAny<Product>())).ReturnsAsync(product);

            // Act
            var ex = Assert.ThrowsAsync<ValidationException>(() => service.AddProduct(product));
          
            // Assert
           Assert.AreEqual(ex.Message, "Bids can't be added while creating a new Product!");
        }

        [Test]
        public async Task DeleteProduct_WithBid_ThrowsException()
        {
            // Arrange
            var service = this.CreateService();
            
            Product product = new Product
            {
                ProductId = 1,
                ProductName = "Very Rare Platinum Necklace",
                ShortDescription = "Platinum Necklace",
                DetailedDescription = "18th Century Necklace worn by many mugal queens",
                Category = Categories.Ornament,
                StartingPrice = 100000,
                BidEndDate = new DateTime(),
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
                {
                    new  Auction.Commom.Model.Buyer
                    {
                        ProductId= 1,
                        BidAmount= 100001,
                        FirstName= "Sundhar",
                        LastName= "Ravi",
                        Address= "Kochi",
                        City= "TVM",
                        State= "KL",
                        Pin= "700232",
                        Phone= "987655321",
                        Email= "PortKochiMehal@example.com"
                    }
                }
            };

            mockProductRepository.Setup(pr => pr.GetProduct(It.IsAny<int>())).ReturnsAsync(product);

            this.mockProductRepository.Setup(x => x.DeleteProduct(It.IsAny<Int32>())).ReturnsAsync(true);
            // Act
            
            var ex = Assert.ThrowsAsync<ValidationException>(() => service.DeleteProduct(product.ProductId));
            // Assert
            Assert.AreEqual(ex.Message, "Product with a bid cannot be deleted");
        }

        [Test]
        public async Task DeleteProduct_With_PastBidDate_ThrowsException()
        {
            // Arrange
            var service = this.CreateService();
            Product product = new Product
            {
                ProductId = 1,
                ProductName = "Very Rare Platinum Necklace",
                ShortDescription = "Platinum Necklace",
                DetailedDescription = "18th Century Necklace worn by many mugal queens",
                Category = Categories.Ornament,
                StartingPrice = 100000,
                BidEndDate = DateTime.Today.AddMinutes(-1),
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

            mockProductRepository.Setup(pr => pr.GetProduct(It.IsAny<int>())).ReturnsAsync(product);

            this.mockProductRepository.Setup(x => x.DeleteProduct(It.IsAny<Int32>())).ReturnsAsync(true);
            // Act
            var ex = Assert.ThrowsAsync<ValidationException>(() => service.DeleteProduct(product.ProductId));
            
            // Assert
            Assert.AreEqual(ex.Message, "Product with completed bid date cannot be delted");
        }

    }
}
