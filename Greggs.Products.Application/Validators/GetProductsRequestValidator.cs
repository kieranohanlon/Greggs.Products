using FluentValidation;
using Greggs.Products.Application.QueryRequest;

namespace Greggs.Products.Application.Validators
{
    public class GetProductsRequestValidator : AbstractValidator<GetProductsQueryRequest>
    {
        public GetProductsRequestValidator()
        {
            this.RuleFor(x => x.PageStart)
                .GreaterThanOrEqualTo(0);

            this.RuleFor(x => x.PageSize)
                .GreaterThan(0);
        }
    }
}
