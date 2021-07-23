using Api.Core.Exceptions;
using AutoMapper;
using PaymentGateway.Api.Core.Data.Dtos;
using PaymentGateway.Api.Core.Data.Service;
using PaymentGateway.Api.Core.Stimulator;
using System;
using System.Threading.Tasks;

namespace PaymentGateway.Api.Core.Service
{
    public class PaymentService : IPaymentService
    {

        private readonly IPaymentRecordDataService _paymentDataService;
        private readonly IMockedBankStimulator _mockedBankStimulator;
        private readonly IMapper _mapper;


        public PaymentService(IPaymentRecordDataService paymentDataService,
            IMockedBankStimulator mockedBankStimulator,
            IMapper mapper)
        {
            _paymentDataService = paymentDataService;
            _mockedBankStimulator = mockedBankStimulator;
            _mapper = mapper;
        }


        public async Task<Response> CreateAsync(CardInformation cardInformation)
        {
            var cardDetails = _mapper.Map<Data.Model.CardDetails>(cardInformation);
            var paymentRecord = _mapper.Map<Data.Model.PaymentRecord>(cardInformation);
            paymentRecord.Card = cardDetails;

            MockedCard result = await _mockedBankStimulator.AcceptPaymentsAsync(cardInformation.CardNumber);

            paymentRecord.Status = result.Status;
            paymentRecord.StatusCode = result.StatusCode;
            paymentRecord.PaymentRecordId = $"sk_{Guid.NewGuid()}";

            var response = await _paymentDataService.CreateAsync(paymentRecord);

            return response ? _mapper.Map<Response>(paymentRecord) : throw new RetryOperationException("Payment request failed. Kindly try the operation.");
        }

        public async Task<PaymentRecord> GetByIdAsync(string paymentRecordId)
        {

            var response = await _paymentDataService.GetByIdAsync(paymentRecordId);

            return response != null ? _mapper.Map<PaymentRecord>(response) : throw new NotFoundException("Could not find the payment details for the specified Id.");
        }
    }
}
