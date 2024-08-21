using AppointmentSystem.Infrastructure.DataAccess;

namespace AppointmentSystem.Infrastructure.Tests;

[Trait("Type","Integration")]
public class CalendarQueryRepositoryTests : IClassFixture<DatabaseFixture>
{
    // Use the pre-seeded data (from init srcipt) in the db to run integration tests
    
    private readonly CalendarQueryRepository _repository;

    public CalendarQueryRepositoryTests(DatabaseFixture fixture)
    {
        _repository = new CalendarQueryRepository(fixture.DbConnectionFactory);
    }

    [Fact]
    public async Task GetAvailableSlotsAsync_SlotsForFirstDay_ReturnsThreeNonOverLappingSlotsWithSalesManagerCount()
    {
        var language = "German";
        var products = new[] { "SolarPanels", "Heatpumps" };
        var customerRating = "Bronze";
        var filterDate = new DateTime(2024, 05, 03);

        var result = await _repository.GetAvailableSlotsAsync(language, products, customerRating, filterDate, CancellationToken.None);

        var availableSlots = result.ToList();
        Assert.NotEmpty(availableSlots);
        Assert.Equal(3, availableSlots.Count());
        Assert.Contains(availableSlots, slot => slot.StartDate == new DateTime(2024, 05, 03, 10, 30, 00) && slot.AvailableCount == 1);
        Assert.Contains(availableSlots, slot => slot.StartDate == new DateTime(2024, 05, 03, 11, 00, 00) && slot.AvailableCount == 1);
        Assert.Contains(availableSlots, slot => slot.StartDate == new DateTime(2024, 05, 03, 11, 30, 00) && slot.AvailableCount == 2);
    }
    
    [Fact]
    public async Task GetAvailableSlotsAsync_SlotsForSecondDay_ReturnsTwoNonOverLappingSlotsWithSalesManagerCount()
    {
        var language = "German";
        var products = new[] { "SolarPanels", "Heatpumps"};
        var customerRating = "Bronze";
        var filterDate = new DateTime(2024, 05, 04);

        var result = await _repository.GetAvailableSlotsAsync(language, products, customerRating, filterDate, CancellationToken.None);

        var availableSlots = result.ToList();
        Assert.NotEmpty(availableSlots);
        Assert.Equal(2, availableSlots.Count());
        Assert.Contains(availableSlots, slot => slot.StartDate == new DateTime(2024, 05, 04, 10, 30, 00) && slot.AvailableCount == 1);
        Assert.Contains(availableSlots, slot => slot.StartDate == new DateTime(2024, 05, 04, 11, 30, 00) && slot.AvailableCount == 1);
    }

    [Fact]
    public async Task GetAvailableSlotsAsync_ShouldReturnEmpty_WhenNoSalesManagerMatchesCriteria()
    {
        var language = "Spanish";
        var products = new[] { "AirCondition" };
        var customerRating = "Gold";
        var filterDate = new DateTime(2024, 05, 03);

        var result = await _repository.GetAvailableSlotsAsync(language, products, customerRating, filterDate, CancellationToken.None);

        Assert.Empty(result);
    }

    [Fact]
    public async Task GetAvailableSlotsAsync_ShouldReturnSlotsForNextDay()
    {
        var language = "German";
        var products = new[] { "Heatpumps" };
        var customerRating = "Gold";
        var filterDate = new DateTime(2024, 05, 04);

        var result = await _repository.GetAvailableSlotsAsync(language, products, customerRating, filterDate, CancellationToken.None);

        var availableSlots = result.ToList();
        Assert.Single(availableSlots);
        Assert.Contains(availableSlots, slot => slot.StartDate == new DateTime(2024, 05, 04, 11, 30, 00));
    }
    
    [Fact]
    public async Task GetAvailableSlotsAsync_WhenNoSlotsAvailableOnDate_ReturnEmpty()
    {
        var language = "German";
        var products = new[] { "SolarPanels", "Heatpumps" };
        var customerRating = "Gold";
        var filterDate = new DateTime(2025, 01, 01);

        var result = await _repository.GetAvailableSlotsAsync(language, products, customerRating, filterDate, CancellationToken.None);

        Assert.Empty(result);
    }

    [Fact]
    public async Task GetAvailableSlotsAsync_WhenPartiallyMatchCriteria_ReturnsData()
    {
        var language = "German";
        var products = new[] { "Heatpumps" , "AirCondition"};
        var customerRating = "Bronze";
        var filterDate = new DateTime(2024, 05, 03);

        var result = await _repository.GetAvailableSlotsAsync(language, products, customerRating, filterDate, CancellationToken.None);

        Assert.NotEmpty(result);
    }
    
    [Fact]
    public async Task GetAvailableSlotsAsync_ShouldReturnEmpty_WhenDateIsFarInTheFutureOrPast()
    {
        var language = "German";
        var products = new[] { "SolarPanels" };
        var customerRating = "Gold";

        var futureDate = new DateTime(3000, 01, 01);
        var resultFuture = await _repository.GetAvailableSlotsAsync(language, products, customerRating, futureDate, CancellationToken.None);
        Assert.Empty(resultFuture);
    
        var pastDate = new DateTime(1900, 01, 01);
        var resultPast = await _repository.GetAvailableSlotsAsync(language, products, customerRating, pastDate, CancellationToken.None);
        Assert.Empty(resultPast);
    }
}
