using AppointmentSystem.Application.Queries;
using AppointmentSystem.Domain.Models;
using AppointmentSystem.Domain.Enums;
using AppointmentSystem.Domain.Errors;
using AppointmentSystem.Infrastructure.DataAccess;
using Moq;

namespace AppointmentSystem.Application.Tests;

[Trait("Type", "Unit")]
public class GetAvailableSlotsQueryHandlerTests
{
    private readonly Mock<ICalendarQueryRepository> _calendarRepositoryMock;
    private readonly GetAvailableSlotsQueryHandler _handler;

    public GetAvailableSlotsQueryHandlerTests()
    {
        _calendarRepositoryMock = new Mock<ICalendarQueryRepository>();
        _handler = new GetAvailableSlotsQueryHandler(_calendarRepositoryMock.Object);
    }

    [Theory]
    [InlineData("unsupportedLanguage")]
    public async Task HandleAsync_InvalidLanguage_ReturnsValidationError(string language)
    {
        // Arrange
        var products = new[] { Product.SolarPanels.ToString() };
        var rating = Rating.Gold.ToString();
        var date = DateOnly.FromDateTime(DateTime.Now);

        // Act
        var result = await _handler.HandleAsync(language, products, rating, date, CancellationToken.None);

        // Assert
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

    [Theory]
    [InlineData("InvalidRating")]
    public async Task HandleAsync_InvalidRating_ReturnsValidationError(string rating)
    {
        // Arrange
        var language = Language.English.ToString();
        var products = new[] { Product.SolarPanels.ToString() };
        var date = DateOnly.FromDateTime(DateTime.Now);

        // Act
        var result = await _handler.HandleAsync(language, products, rating, date, CancellationToken.None);

        // Assert
        var validationError = Assert.IsType<AvailableSlotsRequestValidationError>(result.Value);
        Assert.Equal($"{rating} not supported", validationError.Message);
    }

    [Fact]
    public async Task HandleAsync_NoAvailableSlots_ReturnsNotFoundError()
    {
        // Arrange
        var language = Language.English.ToString();
        var products = new[] { Product.SolarPanels.ToString() };
        var rating = Rating.Gold.ToString();
        var date = DateOnly.FromDateTime(DateTime.Now);

        _calendarRepositoryMock
            .Setup(repo => repo.GetAvailableSlotsAsync(language, products, rating, date.ToDateTime(TimeOnly.MinValue), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Enumerable.Empty<AvailableSlot>());

        // Act
        var result = await _handler.HandleAsync(language, products, rating, date, CancellationToken.None);

        // Assert
        Assert.IsType<AvailableSlotsNotFoundError>(result.Value);
    }

    [Fact]
    public async Task HandleAsync_AvailableSlotsFound_ReturnsAvailableSlots()
    {
        // Arrange
        var language = Language.English.ToString();
        var products = new[] { Product.SolarPanels.ToString() };
        var rating = Rating.Gold.ToString();
        var date = DateOnly.FromDateTime(DateTime.Now);
        var slots = new List<AvailableSlot> { new() };

        _calendarRepositoryMock
            .Setup(repo => repo.GetAvailableSlotsAsync(language, products, rating, date.ToDateTime(TimeOnly.MinValue), It.IsAny<CancellationToken>()))
            .ReturnsAsync(slots);

        // Act
        var result = await _handler.HandleAsync(language, products, rating, date, CancellationToken.None);

        // Assert
        var availableSlots = Assert.IsType<List<AvailableSlot>>(result.Value);
        Assert.Single(availableSlots);
    }

    [Fact]
    public async Task HandleAsync_RepositoryThrowsException_ReturnsExceptionError()
    {
        // Arrange
        var language = Language.English.ToString();
        var products = new[] { Product.SolarPanels.ToString() };
        var rating = Rating.Gold.ToString();
        var date = DateOnly.FromDateTime(DateTime.Now);

        var exception = new Exception("Database connection error");
        _calendarRepositoryMock
            .Setup(repo => repo.GetAvailableSlotsAsync(language, products, rating, date.ToDateTime(TimeOnly.MinValue), It.IsAny<CancellationToken>()))
            .ThrowsAsync(exception);

        // Act
        var result = await _handler.HandleAsync(language, products, rating, date, CancellationToken.None);

        // Assert
        var error = Assert.IsType<AvailableSlotsExceptionError>(result.Value);
        Assert.Equal(exception, error.InnerException);
    }
}