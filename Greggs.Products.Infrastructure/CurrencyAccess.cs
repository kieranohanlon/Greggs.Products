namespace Greggs.Products.Infrastructure
{
    public class CurrencyAccess : ICurrencyAccess
    {
        //TODO - Tidy up this project so classes are in folders etc
        public decimal GetConversionRateFromGBP(string currencyCode)
        {
            return (string.IsNullOrEmpty(currencyCode) || currencyCode.Equals("GBP", StringComparison.OrdinalIgnoreCase)) ? 1 :
                //TODO - Assume that this needs to call a database or external service to get rates
                currencyCode.Equals("EUR", StringComparison.OrdinalIgnoreCase) ? 1.11m : 
                0;
        }
    }
}
