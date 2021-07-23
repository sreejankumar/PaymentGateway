using Api.Core.Commands;
using Api.Core.Exceptions;
using Logging.Extensions;
using PaymentGateway.Api.Core.Data.Dtos;
using PaymentGateway.Api.Core.Service;
using PaymentGateway.Api.Core.Utility;
using System.Threading.Tasks;

namespace PaymentGateway.Api.Core.Commands
{
    public class FetchPaymentsByIdCommand : Command<string, PaymentRecord>
    {
        private readonly IPaymentService _paymentService;

        public FetchPaymentsByIdCommand(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        public override Task<PaymentRecord> Run(string input)
        {
            return _paymentService.GetByIdAsync(input);
        }

        public override Task Validate(string input)
        {
            if(!input.HasValue())
            {
                throw new ValidationException(ExceptionMessage.InvalidParameter(nameof(PaymentRecord.PaymentRecordId)));
            }
            return Task.CompletedTask;
        }
    }
}
