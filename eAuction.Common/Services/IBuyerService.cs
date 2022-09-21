using Auction.Commom.Model;
using System.Threading.Tasks;

namespace Auction.Common.Services
{
    public interface IBuyerService
    {
        Task<Buyer> AddBid(Buyer bid);

        Task<Buyer> UpdateBid(int productId, string email, double amount);
    }
}