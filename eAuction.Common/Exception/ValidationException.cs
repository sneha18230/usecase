namespace Auction.Common.Exception
{
    public class ValidationException : System.Exception
    {
        public ValidationException() : base()
        {
        }

        public ValidationException(string message) : base(message)
        {
        }

    }
}