using Api.Core.Commands;
using PaymentGateway.Api.Core.Data.Dtos;
using PaymentGateway.Api.Core.Service;
using System.Threading.Tasks;

namespace PaymentGateway.Api.Core.Commands
{
    public class ProcessPaymentsCommand : Command<CardInformation, Response>
    {
        private readonly IPaymentService _paymentService;
        

        public ProcessPaymentsCommand(IPaymentService paymentService)
        {
            _paymentService = paymentService;            
        }

        public async override Task<Response> Run(CardInformation input)
        {
            return await _paymentService.CreateAsync(input);
            
        }

        public override Task Validate(CardInformation input)
        {

            input.ValidateRequestModel();
            return Task.CompletedTask;
        }
    }
}
