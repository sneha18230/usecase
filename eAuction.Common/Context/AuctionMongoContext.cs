
using Auction.Commom.Model;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Auction.Commom.Context
{
    public class AuctionMongoContext : IAuctionMongoContext
    {
        private readonly IMongoDatabase database;

        MongoClient client;

        public AuctionMongoContext(IConfiguration configuration)
        {
            client = new MongoClient(configuration.GetSection("MongoDB:ConnectionString").Value);
            database = client.GetDatabase(configuration.GetSection("MongoDB:Database").Value);
            var dbList = client.ListDatabaseNames();
        }

        public MongoCollectionBase<Product> products => (MongoCollectionBase<Product>)database.GetCollection<Product>("products");
    }
}
