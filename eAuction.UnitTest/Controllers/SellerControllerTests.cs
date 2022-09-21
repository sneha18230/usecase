using Auction.Common.Services;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using eAuction.SellerApi.Controllers;
using Auction.Commom.Model;
using Auction.Common.Exception;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace eAuction.UnitTest.Controllers
{
    [TestFixture]
    
    public class SellerControllerTests
    {
        private MockRepository mockRepository;

        private Mock<ILogger<SellerController>> mockLogger;
        private Mock<ISellerService> mockSellerService;

        [SetUp]
        public void SetUp()
        {
            this.mockRepository = new MockRepository(MockBehavior.Default);

            this.mockLogger = this.mockRepository.Create<ILogger<SellerController>>();
            this.mockSellerService = this.mockRepository.Create<ISellerService>();
        }

        private SellerController CreateSellerController()
        {
            return new SellerController(
                this.mockLogger.Object,
                this.mockSellerService.Object);
        }

        [Test]
        public async Task GetProduct_WithValidId_Returns_Product()
        {
            // Arrange
            var sellerController = this.CreateSellerController();
            int id = 1;
            Product prod = getSampleProduct();
            this.mockSellerService.Setup(x => x.GetProductBids(It.IsAny<int>())).ReturnsAsync(prod);

            // Act
            var result = await sellerController.GetProduct(
                id) ;            // Assert


            Assert.IsNotNull(result.Result);
        }

        [Test]
        public async Task GetProduct_WhenNoProduct_for_ProductId_Returns_NotFound()
        {
            // Arrange
            var sellerController = this.CreateSellerController();
            int id = -1 ;
            Product prod = null;
            this.mockSellerService.Setup(x => x.GetProductBids(It.IsAny<int>())).ReturnsAsync(prod);

            // Act
            var result = await sellerController.GetProduct(
                 id);

            // Assert
            Assert.AreEqual( ((NotFoundObjectResult) result.Result).Value, "No Product with given Id");
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
