using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Auction.Commom.Model
{
    public class Product
    {
        [Required]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Only Numbers allowed")]
        [BsonElement("_id")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Required")]
        [StringLength(30, MinimumLength = 5, ErrorMessage = "Min-Max 5-30 chars")]
        public string ProductName { get; set; }

        public string ShortDescription { get; set; }

        public string DetailedDescription { get; set; }

        [Required(ErrorMessage = "Required")]
        [EnumDataType(typeof(Categories), ErrorMessage = "Only Painting,Sculptor,Ornament categories are allowed")]
        public Categories Category { get; set; }

        [Required(ErrorMessage = "Required")]
        public double StartingPrice { get; set; }

        [Required(ErrorMessage = "Required")]
        public DateTime BidEndDate { get; set; }

        [Required(ErrorMessage = "Required")]
        public Seller Seller { get; set; }

        public List<Buyer> Buyers { get; set; }
    }

    public enum Categories
    {
        Painting,
        Sculptor,
        Ornament

    }
}
