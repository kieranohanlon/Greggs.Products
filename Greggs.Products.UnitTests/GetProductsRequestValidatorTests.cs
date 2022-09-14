using Greggs.Products.Application.QueryRequest;
using Greggs.Products.Application.Validators;
using Xunit;

namespace Greggs.Products.UnitTests;

public class GetProductsRequestValidatorTests
{
    private readonly GetProductsRequestValidator validator = new GetProductsRequestValidator();

    //TODO - these tests are fairly simple, would consider using Moq and FluentAssertions if needed
    //TODO - Not all possible unit tests are done here, just some examples

    [Fact]
    public void ValidateWithNegativePageSize()
    {
        var request = new GetProductsQueryRequest()
        {
            PageSize = -3,
            PageStart = 2
        };

        // Act
        var result = this.validator.Validate(request);

        // Assert
        Assert.False(result.IsValid);
    }

    [Fact]
    public void ValidateSuccess()
    {
        // Arrange
        var request = new GetProductsQueryRequest()
        {
            PageSize = 10,
            PageStart = 1
        };

        // Act
        var result = this.validator.Validate(request);

        // Assert
        Assert.True(result.IsValid);
    }
}