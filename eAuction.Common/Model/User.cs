using System.ComponentModel.DataAnnotations;

namespace Auction.Commom.Model
{
    public class User
    {
        [Required(ErrorMessage = "Required")]
        [StringLength(30, MinimumLength = 5, ErrorMessage = "Min-Max 5-30 chars")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Required")]
        [StringLength(25, MinimumLength = 3, ErrorMessage = "Min-Max 3-25 chars")]
        public string LastName { get; set; }

        public string Address { get; set; }

        [RegularExpression("^[a-zA-Z]*$", ErrorMessage = "Alphabets Only Field")]
        public string City { get; set; }

        [RegularExpression("^[a-zA-Z]*$", ErrorMessage = "Alphabets Only Field")]
        public string State { get; set; }

        [RegularExpression("^[0-9]*$", ErrorMessage = "Numbers Only Field")]
        public string Pin { get; set; }

        [RegularExpression("^[0-9]*$", ErrorMessage = "Numbers Only Field")]
        [Required(ErrorMessage = "Required")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Required")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; }
    }
}
