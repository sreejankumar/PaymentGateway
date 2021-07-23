using PaymentGateway.Api.Core.Data.Model;
using System;
using System.Threading.Tasks;

namespace PaymentGateway.Api.Core.Data.Service
{
    public interface IPaymentRecordDataService
    {
        Task<bool> CreateAsync(PaymentRecord paymentRecord);
        Task<PaymentRecord> GetByIdAsync(string paymentRecordId);
    }
}
