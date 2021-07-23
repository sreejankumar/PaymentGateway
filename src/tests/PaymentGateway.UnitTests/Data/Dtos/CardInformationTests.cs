using Api.Core.Exceptions;
using FluentAssertions;
using NUnit.Framework;
using PaymentGateway.Api.Core.Data.Dtos;
using PaymentGateway.Api.Core.Data.Enums;
using PaymentGateway.Api.Core.Utility;

namespace PaymentGateway.UnitTests.Data.Dtos
{
    [TestFixture]
    public class CardInformationTests
    {

        private CardInformation cardInfo;
        [SetUp]
        public void Setup()
        {
            cardInfo = new CardInformation()
            {
                Amount = 10,
                CardNumber = "4543474002249996",
                CardType = CardType.Visa,
                ClientId = "123",
                Currency = "USD",
                Cvv = 123,
                ExpiryMonth = 12,
                ExpiryYear = 2021
            };
        }


        [TestCase("USD1")]
        [TestCase("GBP2")]
        public void Validate_Currency_Non_ISO(string input)
        {

            cardInfo.Currency = input;

            cardInfo.Invoking(x => x.ValidateRequestModel()).Should().Throw<ValidationException>()
                .WithMessage(ExceptionMessage.InvalidCurrencyMessage);
        }

        [TestCase("")]
        public void Validate_Currency_Empty(string input)
        {

            cardInfo.Currency = input;

            cardInfo.Invoking(x => x.ValidateRequestModel()).Should().Throw<ValidationException>()
                .WithMessage((ExceptionMessage.InvalidParameter($"{nameof(CardInformation.ExpiryMonth)} or {nameof(CardInformation.ExpiryYear)} or " +
                    $"{nameof(CardInformation.CardNumber)} or {nameof(CardInformation.Cvv)} or {nameof(CardInformation.Currency)} or {nameof(CardInformation.Currency)}")));
        }


        [TestCase(123)]

        public void Validate_CardType_Has_Value(int? input)
        {

            cardInfo.Currency = "USD";


            cardInfo.CardType = (CardType)input;
            cardInfo.Invoking(x => x.ValidateRequestModel()).Should().Throw<ValidationException>()
                .WithMessage(ExceptionMessage.InvalidParameter(nameof(CardType),
                    cardInfo.CardType, CardInformation.BrandValues));
        }

        [TestCase("4169773331987017")]
        [TestCase("4658958254583145")]
        [TestCase("4771320594033780")]
        public void Validate_CardType_Is_Visa(string cardNumber)
        {

            cardInfo.CardNumber = cardNumber;

            cardInfo.ValidateRequestModel();
            cardInfo.CardType.Should().Be(CardType.Visa);
        }

        [TestCase("5410710000901089")]
        [TestCase("5289675573349651")]
        [TestCase("5582128534772839")]
        public void Validate_CardType_Is_MasterCard(string cardNumber)
        {

            cardInfo.CardNumber = cardNumber;

            cardInfo.ValidateRequestModel();
            cardInfo.CardType.Should().Be(CardType.MasterCard);
        }

        [TestCase("349101032764066")]
        [TestCase("343042534582349")]
        [TestCase("371305972529535")]
        public void Validate_CardType_Is_AmericanExpress(string cardNumber)
        {
            cardInfo.CardNumber = cardNumber;
            cardInfo.ValidateRequestModel();
            cardInfo.CardType.Should().Be(CardType.AmericanExpress);
        }
    }
}
