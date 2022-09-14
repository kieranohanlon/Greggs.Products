using FluentValidation;
using Greggs.Products.Application.QueryRequest;
using Greggs.Products.Models;
using MediatR;

namespace Greggs.Products.Application.QueryHandlers
{
    public class GetProductsQueryHandler : IRequestHandler<GetProductsQueryRequest, IEnumerable<Product>>
    {
        private readonly IValidator<GetProductsQueryRequest> _validator;

        private static readonly string[] Products = new[]
        {
            "Sausage Roll", "Vegan Sausage Roll", "Steak Bake", "Yum Yum", "Pink Jammie"
        };

        public GetProductsQueryHandler(IValidator<GetProductsQueryRequest> validator)
        {
            _validator = validator;
        }

        public async Task<IEnumerable<Product>> Handle(GetProductsQueryRequest request, CancellationToken cancellationToken)
        {
            _validator.ValidateAndThrow(request);

            if (request.PageSize > Products.Length)
                request.PageSize = Products.Length;

            //TODO - Consider caching, possibly using Redis

            var rng = new Random();           

            //TODO - would probably look to return a response products class rather than the model directly, to decouple it, use automapper for this.

            return await Task.FromResult(Enumerable.Range(1, request.PageSize).Select(index => new Product
            {
                PriceInPounds = rng.Next(0, 10),
                Name = Products[rng.Next(Products.Length)]
            })
            .ToArray());
        }
    }
}
