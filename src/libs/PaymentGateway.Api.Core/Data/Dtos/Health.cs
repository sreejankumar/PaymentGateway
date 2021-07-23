namespace PaymentGateway.Api.Core.Data.Dtos
{
    public class Health
    {
        public string Status { get; set; }
        public string Name { get; set; }
        public bool Timeout { get; set; }
        public int Nodes { get; set; }
        public long WaitTime { get; set; }
    }
}
