
using Auction.Commom.Model;
using MongoDB.Driver;

namespace Auction.Commom.Context
{
    public interface IAuctionMongoContext
    {
        MongoCollectionBase<Product> products { get; }
    }
}
