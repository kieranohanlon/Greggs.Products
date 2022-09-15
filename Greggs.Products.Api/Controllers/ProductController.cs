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
    private readonly ILogger<ProductController> _logger;
    private readonly IMediator _mediator;

    public ProductController(ILogger<ProductController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    //TODO - Consider if any authentication and authorisation is needed?

    //TODO - For User Story 2 I'd need to confirm that using the same overloaded "Get" endpoint is acceptable and that they are happy with a pagenated request/response.
    //I have also assumed that it's possible that in the future other currencies may need to be added, this could be confirmed.
    //If it's definite that the products would only ever be returned for GBP and EURO then I would possibly just have a separate endpoint for EURO,
    //or have a boolean flag for EURO in GetProductsQueryRequest rather than currency code and made the User Story 2 even simpler

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
        //Implemented clean architecture – using MediatR and going down the route of the CQRS pattern (assuming this API will have
        //writes to database in the future, if not then possibly change this just to use MediatR)
        //I have also assumed that this API will grow significantly, it's possible this is over engineered if it were to remain as simple as it is now,
        //in which case a more traditional service and repository design may be easier for people follow.
        var result = await _mediator.Send(request);
        return this.Ok(result);
    }
}