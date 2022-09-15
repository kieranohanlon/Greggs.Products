using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Greggs.Products.Infrastructure
{
    public interface ICurrencyAccess
    {
        decimal GetConversionRateFromGBP(string CurrencyCode);
    }
}
