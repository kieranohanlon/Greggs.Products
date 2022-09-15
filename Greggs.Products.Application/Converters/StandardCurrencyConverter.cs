using Greggs.Products.Infrastructure;

namespace Greggs.Products.Application.Converters
{
    public class StandardCurrencyConverter : CurrencyConverterBase
    {
        public StandardCurrencyConverter(ICurrencyAccess currencyAccess):base(currencyAccess)
        {
        }
        public override async Task<decimal> ConvertFromGBP(decimal gbpValue, string currencyCode)
        {
            var conversionRate = await Task.FromResult(_currencyAccess.GetConversionRateFromGBP(currencyCode));

            return gbpValue * conversionRate;
        }
    }
}
