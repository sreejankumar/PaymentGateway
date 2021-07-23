using PaymentGateway.Api.Core.Data.Dtos;
using System;
using System.Threading.Tasks;

namespace PaymentGateway.Api.Core.Service
{
    public interface IPaymentService
    {
        Task<Response> CreateAsync(CardInformation cardInformation);
        Task<Data.Dtos.PaymentRecord> GetByIdAsync(string paymentRecordId);
    }
}