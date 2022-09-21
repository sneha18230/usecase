using System;
using System.ComponentModel.DataAnnotations;

namespace Auction.Commom.Model
{
    public class Buyer : User
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        [Range(1, double.PositiveInfinity, ErrorMessage = "Bid Amount should be greater than 1")]
        public double BidAmount { get; set; }

    }
}
