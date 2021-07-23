using System;

namespace PaymentGateway.Api.Core.Data.Dtos
{
    public class PaymentRecord
    {
        public string PaymentRecordId { get; set; }
        public CardDetails Card { get; set; }
        public string ClientId { get; set; }       
        public float Amount { get; set; }
        public string Currency { get; set; }
        public string Status { get; set; }        
        public int StatusCode { get; set; }
    }
}
