using PaymentGateway.Api.Core.Data.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway.Api.Core.Stimulator
{
    public class MockedBankStimulator : IMockedBankStimulator
    {
        public List<MockedCard> MockedCards { get; set; }
        public MockedBankStimulator()
        {
            MockedCards = new List<MockedCard>
            {
                new MockedCard
                {
                    CardNumber ="4242424242424242",
                   Status = Status.Approved.ToString(),
                    StatusCode = (int)Status.Approved
                },
                new MockedCard
                {
                    CardNumber ="4543474002249996",
                    Status = Status.Declined.ToString(),
                    StatusCode = (int)Status.Declined
                },
                 new MockedCard
                {
                    CardNumber ="5436031030606378	",
                   Status = Status.SoftDecline.ToString(),
                    StatusCode = (int)Status.SoftDecline
                }
            };
        }

        public Task<MockedCard> AcceptPaymentsAsync(string cardNumber)
        {
            var result = MockedCards.FirstOrDefault(x => x.CardNumber == cardNumber);          
            return Task.FromResult(result);
        }      
    }
}
