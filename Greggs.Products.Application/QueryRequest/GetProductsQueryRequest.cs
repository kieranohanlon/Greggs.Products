using Greggs.Products.Models;
using MediatR;

namespace Greggs.Products.Application.QueryRequest
{
    public class GetProductsQueryRequest : IRequest<IEnumerable<Product>>
    {
        public int PageStart { get; set; }
        public int PageSize { get; set; }
    }
}
