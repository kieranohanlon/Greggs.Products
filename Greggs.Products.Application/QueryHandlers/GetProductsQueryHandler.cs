using FluentValidation;
using Greggs.Products.Application.QueryRequest;
using Greggs.Products.Infrastructure;
using Greggs.Products.Models;
using MediatR;
using Microsoft.Extensions.Configuration;
using Greggs.Products.Application.Config;
using Greggs.Products.Application.Converters;

namespace Greggs.Products.Application.QueryHandlers
{
    public class GetProductsQueryHandler : IRequestHandler<GetProductsQueryRequest, IEnumerable<Product>>
    {
        private readonly IValidator<GetProductsQueryRequest> _validator;
        private readonly IDataAccess<Product> _dataAccess;
        private readonly ICurrencyAccess _currencyAccess;
        private readonly IConfiguration _configuration;

        public GetProductsQueryHandler(IValidator<GetProductsQueryRequest> validator, IDataAccess<Product> dataAccess,
             ICurrencyAccess currencyAccess, IConfiguration configuration)
        {
            _validator = validator;
            _dataAccess = dataAccess;
            _currencyAccess = currencyAccess;
            _configuration = configuration;
        }

        public async Task<IEnumerable<Product>> Handle(GetProductsQueryRequest request, CancellationToken cancellationToken)
        {
            _validator.ValidateAndThrow(request);

            var TotalProducts = _dataAccess.TotalProducts();

            //Not sure this is really needed, but leaving in to match the original code.
            if ((request.PageStart + request.PageSize) > TotalProducts)
                request.PageSize = TotalProducts - request.PageStart;

            //TODO - Consider caching, possibly using Redis  
            //TODO - Possibly look to return a response products class rather than the model directly, to decouple it, use automapper for this.

            var products = await Task.FromResult(_dataAccess.List(request.PageStart, request.PageSize));

#pragma warning disable CS8604 // Possible null reference argument.
            return await ConvertProductPrices(products, request.CurrencyCode);
#pragma warning restore CS8604 // Possible null reference argument.
        }

        private async Task<IEnumerable<Product>> ConvertProductPrices(IEnumerable<Product> products, string convertToCurrencyCode)
        {
            //TODO - This would probably be done in a nicer way, maybe using a factory to create and possibly using
            //attributes and reflection to create the correct concrete converter class rather than the loop below

            //Using the config to decide which currencies map to which converters, so if any new currencies need to be
            //added and they use the same converter class, it's just a simple config change that's needed
            var configCurrencyConverters = _configuration.GetSection("CurrencyConverters").Get<CurrencyConverters>();

            //Using a strategy pattern to determine the conveter to use - the advantage here is that we could add in
            //new convertors for other currencies in the future, which could be different to the standard one if 
            //need - e.g. add an extra margin or even source the conversion rate from a different source etc

            if (string.IsNullOrEmpty(convertToCurrencyCode))
            {
                convertToCurrencyCode = "GBP";
            }

            var updatedPrice = false;
            foreach (var configCurrencyCode in configCurrencyConverters.StandardCurrencyConverter.CurrencyCodes)
            {
                if(convertToCurrencyCode.Equals(configCurrencyCode, StringComparison.OrdinalIgnoreCase))
                {
                    var currencyConverter = new StandardCurrencyConverter(_currencyAccess);
                    foreach(var product in products)
                    {
                        product.Price = await currencyConverter.ConvertFromGBP(product.PriceInPounds, convertToCurrencyCode);
                        product.PriceIsInCurrencyCode = convertToCurrencyCode.ToUpper();
                    }
                    updatedPrice = true;
                }
            }

            if(!updatedPrice)
            {
                throw new ArgumentException($"Currency supplied '{convertToCurrencyCode}' is invalid");
            }

            return products;
        }
    }
}
