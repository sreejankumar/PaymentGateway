namespace PaymentGateway.Api.Core.Stimulator
{
    public class MockedCard
    {
        public string CardNumber { get; set; }
        public int StatusCode { get; set; }
        public string Status { get; internal set; }
    }
}
