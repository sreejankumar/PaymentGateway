using System.Threading.Tasks;

namespace PaymentGateway.Api.Core.Stimulator
{
    public interface IMockedBankStimulator
    {


        Task<MockedCard> AcceptPaymentsAsync(string cardNumber);

    }
}