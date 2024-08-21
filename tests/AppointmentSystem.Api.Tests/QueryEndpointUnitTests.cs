using AppointmentSystem.Api.Endpoints.Calendar;
using AppointmentSystem.Application.Queries;
using AppointmentSystem.Domain.Errors;
using AppointmentSystem.Domain.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;

namespace AppointmentSystem.Api.Tests;

[Trait("Type","Unit")]
public class QueryEndpointUnitTests
{
    private readonly Mock<IGetAvailableSlotsQueryHandler> _mockQueryHandler;
    private readonly QueryEndpoint _endpoint;

    public QueryEndpointUnitTests()
    {
        _mockQueryHandler = new Mock<IGetAvailableSlotsQueryHandler>();
        _endpoint = new QueryEndpoint(_mockQueryHandler.Object);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnBadRequest_WhenLanguageIsEmpty()
    {
        var request = new QueryRequest ("2024-05-03", new[] { "SolarPanels" },"" ,"Gold" );
        
        var result = await _endpoint.ExecuteAsync(request, CancellationToken.None);
        
        Assert.IsType<BadRequest<string>>(result.Result);
        Assert.Equal("Language cannot be empty or whitespace", ((BadRequest<string>)result.Result).Value);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnBadRequest_WhenNoProductsSpecified()
    {
        var request = new QueryRequest ( "German",  Array.Empty<string>(), "Gold",  "2024-05-03" );
        
        var result = await _endpoint.ExecuteAsync(request, CancellationToken.None);
        
        Assert.IsType<BadRequest<string>>(result.Result);
        Assert.Equal("At least one product must be specified", ((BadRequest<string>)result.Result).Value);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnBadRequest_WhenRatingIsEmpty()
    {
        var request = new QueryRequest("2024-05-03", new[] { "SolarPanels" }, "German","" );
        
        var result = await _endpoint.ExecuteAsync(request, CancellationToken.None);
        
        Assert.IsType<BadRequest<string>>(result.Result);
        Assert.Equal("Rating cannot be empty or whitespace", ((BadRequest<string>)result.Result).Value);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnBadRequest_WhenDateFormatIsInvalid()
    {
        var request = new QueryRequest("German", new[] { "SolarPanels" }, "Gold", "invalid-date" );
        
        var result = await _endpoint.ExecuteAsync(request, CancellationToken.None);
        
        Assert.IsType<BadRequest<string>>(result.Result);
        Assert.Equal("Date can only be entered in yyyy-MM-dd", ((BadRequest<string>)result.Result).Value);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnNotFound_WhenAvailableSlotsNotFound()
    {
        var request = new QueryRequest ("2024-05-03" , new[] { "SolarPanels" },  "German","Gold");
        
        _mockQueryHandler
            .Setup(x => x.HandleAsync(request.Language, request.Products, request.Rating, It.IsAny<DateOnly>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AvailableSlotsNotFoundError());

        var result = await _endpoint.ExecuteAsync(request, CancellationToken.None);
        
        Assert.IsType<NotFound>(result.Result);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnProblem_WhenExceptionOccurs()
    {
        var request = new QueryRequest( "2024-05-03", new[] { "SolarPanels" }, "German", "Gold");
        
        _mockQueryHandler
            .Setup(x => x.HandleAsync(request.Language, request.Products, request.Rating, It.IsAny<DateOnly>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AvailableSlotsExceptionError(new Exception("Unexpected Error")));

        var result = await _endpoint.ExecuteAsync(request, CancellationToken.None);
        
        var problemResult = Assert.IsType<ProblemHttpResult>(result.Result);
        Assert.Equal(500, problemResult.StatusCode);
        Assert.Equal("Unexpected Error", problemResult.ProblemDetails.Title);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnBadRequest_WhenValidationFails()
    {
        var request = new QueryRequest("", new[] { "SolarPanels" }, "", "");
        
        _mockQueryHandler
            .Setup(x => x.HandleAsync(request.Language, request.Products, request.Rating, It.IsAny<DateOnly>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AvailableSlotsRequestValidationError("Invalid request"));

        var result = await _endpoint.ExecuteAsync(request, CancellationToken.None);
        
        Assert.IsType<BadRequest<string>>(result.Result);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnOk_WithAvailableSlots()
    {
        var request = new QueryRequest ("2024-05-03", new[] { "SolarPanels" },"German","Gold");
        List<AvailableSlot> slots = [
            new AvailableSlot{ AvailableCount = 1, StartDate = DateTime.Now },
            new AvailableSlot{ AvailableCount = 2, StartDate = DateTime.Now.AddDays(1) },
        ];
        
        _mockQueryHandler 
            .Setup(x => x.HandleAsync(request.Language, request.Products, request.Rating, It.IsAny<DateOnly>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(slots);

        var result = await _endpoint.ExecuteAsync(request, CancellationToken.None);
        
        var okResult = Assert.IsType<Ok<QueryResponse>>(result.Result);
        Assert.NotNull(okResult.Value);
        Assert.Equal(slots, okResult.Value.AvailableSlots);
    }
}
