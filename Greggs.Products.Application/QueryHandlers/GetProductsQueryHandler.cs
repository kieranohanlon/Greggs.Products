using FluentValidation;
using Greggs.Products.Application.QueryRequest;
using Greggs.Products.Infrastructure;
using Greggs.Products.Models;
using MediatR;

namespace Greggs.Products.Application.QueryHandlers
{
    public class GetProductsQueryHandler : IRequestHandler<GetProductsQueryRequest, IEnumerable<Product>>
    {
        private readonly IValidator<GetProductsQueryRequest> _validator;
        private readonly IDataAccess<Product> _dataAccessService;

        public GetProductsQueryHandler(IValidator<GetProductsQueryRequest> validator, IDataAccess<Product> dataAccessService)
        {
            _validator = validator;
            _dataAccessService = dataAccessService;
        }

        public async Task<IEnumerable<Product>> Handle(GetProductsQueryRequest request, CancellationToken cancellationToken)
        {
            _validator.ValidateAndThrow(request);

            var TotalProducts = _dataAccessService.TotalProducts();

            //Not sure this is really needed, but leaving in to match the original code.
            if ((request.PageStart + request.PageSize) > TotalProducts)
                request.PageSize = TotalProducts - request.PageStart;

            //TODO - Consider caching, possibly using Redis  
            //TODO - would probably look to return a response products class rather than the model directly, to decouple it, use automapper for this.

            return await Task.FromResult(_dataAccessService.List(request.PageStart, request.PageSize));
        }
    }
}
