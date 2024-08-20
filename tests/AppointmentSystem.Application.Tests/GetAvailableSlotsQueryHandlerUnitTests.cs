using AppointmentSystem.Application.Queries;
using AppointmentSystem.Domain.Enums;
using AppointmentSystem.Domain.Errors;
using AppointmentSystem.Domain.Models;
using AppointmentSystem.Infrastructure.DataAccess;
using Moq;

namespace AppointmentSystem.Application.Tests;

[Trait("Type", "Unit")]
public class GetAvailableSlotsQueryHandlerUnitTests
{
    private readonly Mock<ICalendarQueryRepository> _calendarRepositoryMock;
    private readonly GetAvailableSlotsQueryHandler _handler;

    public GetAvailableSlotsQueryHandlerUnitTests()
    {
        _calendarRepositoryMock = new Mock<ICalendarQueryRepository>();
        _handler = new GetAvailableSlotsQueryHandler(_calendarRepositoryMock.Object);
    }

    [Fact]
    public async Task HandleAsync_InvalidLanguage_ReturnsValidationError()
    {
        var language = "unsupportedLanguage";
        var products = new[] { Product.SolarPanels.ToString() };
        var rating = Rating.Gold.ToString();
        var date = DateOnly.FromDateTime(DateTime.Now);

        var result = await _handler.HandleAsync(language, products, rating, date, CancellationToken.None);

        var validationError = Assert.IsType<AvailableSlotsRequestValidationError>(result.Value);
        Assert.Equal($"{language} not supported", validationError.Message);
    }

    [Fact]
    public async Task HandleAsync_InvalidProduct_ReturnsValidationError()
    {
        // Arrange
        var language = Language.English.ToString();
        var products = new[] { Product.SolarPanels.ToString(), "InvalidProduct" };
        var rating = Rating.Gold.ToString();
        var date = DateOnly.FromDateTime(DateTime.Now);

        // Act
        var result = await _handler.HandleAsync(language, products, rating, date, CancellationToken.None);

        // Assert
        var validationError = Assert.IsType<AvailableSlotsRequestValidationError>(result.Value);
        Assert.Equal("One or more products are not supported", validationError.Message);
    }

    [Fact]
    public async Task HandleAsync_InvalidRating_ReturnsValidationError()
    {
        var rating = "InvalidRating";
        var language = Language.English.ToString();
        var products = new[] { Product.SolarPanels.ToString() };
        var date = DateOnly.FromDateTime(DateTime.Now);

        var result = await _handler.HandleAsync(language, products, rating, date, CancellationToken.None);

        var validationError = Assert.IsType<AvailableSlotsRequestValidationError>(result.Value);
        Assert.Equal($"{rating} not supported", validationError.Message);
    }

    [Fact]
    public async Task HandleAsync_NoAvailableSlots_ReturnsNotFoundError()
    {
        var language = Language.English.ToString();
        var products = new[] { Product.SolarPanels.ToString() };
        var rating = Rating.Gold.ToString();
        var date = DateOnly.FromDateTime(DateTime.Now);

        _calendarRepositoryMock
            .Setup(repo => repo.GetAvailableSlotsAsync(language, products, rating, date.ToDateTime(TimeOnly.MinValue), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Enumerable.Empty<AvailableSlot>());

        var result = await _handler.HandleAsync(language, products, rating, date, CancellationToken.None);

        Assert.IsType<AvailableSlotsNotFoundError>(result.Value);
    }

    [Fact]
    public async Task HandleAsync_AvailableSlotsFound_ReturnsAvailableSlots()
    {
        var language = Language.English.ToString();
        var products = new[] { Product.SolarPanels.ToString() };
        var rating = Rating.Gold.ToString();
        var date = DateOnly.FromDateTime(DateTime.Now);
        var slots = new List<AvailableSlot> { new() };

        _calendarRepositoryMock
            .Setup(repo => repo.GetAvailableSlotsAsync(language, products, rating, date.ToDateTime(TimeOnly.MinValue), It.IsAny<CancellationToken>()))
            .ReturnsAsync(slots);

        var result = await _handler.HandleAsync(language, products, rating, date, CancellationToken.None);

        var availableSlots = Assert.IsType<List<AvailableSlot>>(result.Value);
        Assert.Single(availableSlots);
    }

    [Fact]
    public async Task HandleAsync_RepositoryThrowsException_ReturnsExceptionError()
    {
        var language = Language.English.ToString();
        var products = new[] { Product.SolarPanels.ToString() };
        var rating = Rating.Gold.ToString();
        var date = DateOnly.FromDateTime(DateTime.Now);

        var exception = new Exception("Database connection error");
        _calendarRepositoryMock
            .Setup(repo => repo.GetAvailableSlotsAsync(language, products, rating, date.ToDateTime(TimeOnly.MinValue), It.IsAny<CancellationToken>()))
            .ThrowsAsync(exception);

        var result = await _handler.HandleAsync(language, products, rating, date, CancellationToken.None);

        var error = Assert.IsType<AvailableSlotsExceptionError>(result.Value);
        Assert.Equal(exception, error.InnerException);
    }
}