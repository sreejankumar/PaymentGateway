using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PaymentGateway.Api.Core.Utility
{
    public class ExceptionMessage
    {
        public static string InvalidCurrencyMessage =
             "Invalid Currency code. Supported format is: ISO 4217";

        public static string EmptyCurrencyMessage =
             "Currency cannot be null or empty. Supported format is: ISO 4217";

     
        public static string InvalidParameter(string parameterName, object value, Dictionary<int, string> defaultValue)
        {
            var values = defaultValue.Select(kvp => kvp.Key + ": " + kvp.Value.ToString());
            return
                $"Parameter {parameterName}:{value} is not valid input." +
                $" Parameter need to match this list {string.Join(",", values)}.";
        }

        public static string InvalidParameter(string parameterName )
        {
            return
                $"Parameter {parameterName} cannot be nonzero";
        }
    }
}
