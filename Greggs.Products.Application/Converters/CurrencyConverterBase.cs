using Greggs.Products.Infrastructure;

namespace Greggs.Products.Application.Converters
{
    public abstract class CurrencyConverterBase
    {
        internal readonly ICurrencyAccess _currencyAccess;
        public CurrencyConverterBase(ICurrencyAccess currencyAccess)
        {
            _currencyAccess = currencyAccess;
        }

        public abstract Task<decimal> ConvertFromGBP(decimal gbpValue, string CurrencyCode);
    }
}
