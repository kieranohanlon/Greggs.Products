using Greggs.Products.Application.QueryRequest;
using Greggs.Products.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Greggs.Products.Api.Controllers;

//TODO - Add XML - find all build warnings and add as needed
[ApiController]
[ApiVersion("1.0")]
[Route("v{version:apiVersion}/[controller]")]
[Produces("application/json")]
public class ProductController : ControllerBase
{
    private static readonly string[] Products = new[]
    {
        "Sausage Roll", "Vegan Sausage Roll", "Steak Bake", "Yum Yum", "Pink Jammie"
    };

    private readonly ILogger<ProductController> _logger;
    private readonly IMediator _mediator;

    public ProductController(ILogger<ProductController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    //TODO - Consider if any authentication and authorisation is needed?

    /// <summary>
    /// Get the latest menu of products
    /// </summary>
    /// <param name="request">GetProductsQueryRequest</param>
    /// <returns>list of Products</returns>
    [ProducesResponseType(typeof(IEnumerable<Product>), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    [HttpGet("Get")]
    public async Task<ActionResult> Get([FromQuery]GetProductsQueryRequest request)
    {
        _logger.LogInformation("ProductController:(int pageStart = {0}, int pageSize = {1})", request.PageStart, request.PageSize);
        //Implemented clean architecture – using MediatR and going down the route of the CQRS pattern (if have writes to database, if not then possibly change this just to use MediatR)
        var result = await _mediator.Send(request);
        return this.Ok(result);
    }
}