using Api.Core.Exceptions;
using Logging.Extensions;
using PaymentGateway.Api.Core.Data.Enums;
using PaymentGateway.Api.Core.Utility;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace PaymentGateway.Api.Core.Data.Dtos
{
    public class CardInformation
    {

        private readonly IEnumerable<string> CurrencyList = CultureInfo.GetCultures(CultureTypes.AllCultures)
           .Where(c => !c.IsNeutralCulture)
           .Select(culture =>
           {
               try
               {
                   return new RegionInfo(culture.Name);
               }
               catch
               {
                   return null;
               }
           })
           .Where(ri => ri != null)
           .GroupBy(ri => ri.ISOCurrencySymbol).Select(x => x.Key);

        public static readonly Dictionary<int, string> BrandValues = System.Enum.GetValues(typeof(CardType)).
            Cast<CardType>().ToDictionary(t => (int)t, t => t.ToString());

        public void ValidateRequestModel()
        {
            ValidateEmptyParamters();
            ValidateCurrency();
            ValidateAndPopulateCardType();
        }

        private void ValidateCurrency()
        {
            if (!CurrencyList.Contains(Currency))
            {
                throw new ValidationException(ExceptionMessage.InvalidCurrencyMessage);
            }
        }

        private void ValidateAndPopulateCardType()
        {
            if (CardType.HasValue && !System.Enum.IsDefined(typeof(CardType), CardType))
            {
                throw new ValidationException(ExceptionMessage.InvalidParameter(nameof(CardType),
                    CardType.Value, BrandValues));
            }


            if (Regex.Match(CardNumber, @"^4[0-9]{12}(?:[0-9]{3})?$").Success)
            {
                CardType = Enums.CardType.Visa;
            }

            else if (Regex.Match(CardNumber, @"^(?:5[1-5][0-9]{2}|222[1-9]|22[3-9][0-9]|2[3-6][0-9]{2}|27[01][0-9]|2720)[0-9]{12}$").Success)
            {
                CardType = Enums.CardType.MasterCard;
            }

            else if (Regex.Match(CardNumber, @"^3[47][0-9]{13}$").Success)
            {
                CardType = Enums.CardType.AmericanExpress;
            }
            else
            {
                    throw new ValidationException(ExceptionMessage.InvalidParameter(nameof(CardType),
                    CardType.Value, BrandValues));
            }
        }

        private void ValidateEmptyParamters()
        {
            if (!ExpiryMonth.HasValue || !ExpiryYear.HasValue || !CardNumber.HasValue() || !Cvv.HasValue || !Amount.HasValue || !Currency.HasValue())
            {
                throw new ValidationException(ExceptionMessage.InvalidParameter($"{nameof(ExpiryMonth)} or {nameof(ExpiryYear)} or " +
                    $"{nameof(CardNumber)} or {nameof(Cvv)} or {nameof(Currency)} or {nameof(Currency)}"));
            }
        }


        public CardType? CardType { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        public int? ExpiryMonth { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        public int? ExpiryYear { get; set; }
        [System.ComponentModel.DataAnnotations.Required]
        public string CardNumber { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        public int? Cvv { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        public string ClientId { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        public float? Amount { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        public string Currency { get; set; }
    }
}
