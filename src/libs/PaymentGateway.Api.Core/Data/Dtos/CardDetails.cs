
namespace PaymentGateway.Api.Core.Data.Dtos
{
    public class CardDetails
    {
        public string CardType { get; set; }
        public int ExpiryMonth { get; set; }
        public int ExpiryYear { get; set; }
        public string LastFourCard { get; set; }        
    }
}
