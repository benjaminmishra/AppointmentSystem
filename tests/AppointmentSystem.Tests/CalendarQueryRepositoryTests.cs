using AppointmentSystem.Infrastructure.DataAccess;

namespace AppointmentSystem.Tests;

[Trait("Type", "Integration")]
public class CalendarQueryRepositoryTests : IClassFixture<DatabaseFixture>
{
    private readonly CalendarQueryRepository _repository;

    public CalendarQueryRepositoryTests(DatabaseFixture fixture)
    {
        _repository = new CalendarQueryRepository(fixture.DbConnectionFactory);
    }

    [Fact]
    public async Task GetAvailableSlotsAsync_Monday20240503_SolarPanelsAndHeatpumps_GermanAndGoldCustomer_OnlySeller2Selectable()
    {
        var language = "German";
        var products = new[] { "SolarPanels", "Heatpumps" };
        var customerRating = "Gold";
        var filterDate = new DateTime(2024, 05, 03);

        var result = await _repository.GetAvailableSlotsAsync(language, products, customerRating, filterDate, CancellationToken.None);

        var availableSlots = result.ToList();
        Assert.Equal(3, availableSlots.Count);
        Assert.Contains(availableSlots, slot => slot.StartDate == new DateTime(2024, 05, 03, 10, 30, 00) && slot.AvailableCount == 1);
        Assert.Contains(availableSlots, slot => slot.StartDate == new DateTime(2024, 05, 03, 11, 00, 00) && slot.AvailableCount == 1);
        Assert.Contains(availableSlots, slot => slot.StartDate == new DateTime(2024, 05, 03, 11, 30, 00) && slot.AvailableCount == 1);
    }

    [Fact]
    public async Task GetAvailableSlotsAsync_Monday20240503_Heatpumps_EnglishAndSilverCustomer_BothSeller2And3Selectable()
    {
        var language = "English";
        var products = new[] { "Heatpumps" };
        var customerRating = "Silver";
        var filterDate = new DateTime(2024, 05, 03);

        var result = await _repository.GetAvailableSlotsAsync(language, products, customerRating, filterDate, CancellationToken.None);

        var availableSlots = result.ToList();
        Assert.Equal(3, availableSlots.Count);
        Assert.Contains(availableSlots, slot => slot.StartDate == new DateTime(2024, 05, 03, 10, 30, 00) && slot.AvailableCount == 1);
        Assert.Contains(availableSlots, slot => slot.StartDate == new DateTime(2024, 05, 03, 11, 00, 00) && slot.AvailableCount == 1);
        Assert.Contains(availableSlots, slot => slot.StartDate == new DateTime(2024, 05, 03, 11, 30, 00) && slot.AvailableCount == 2);
    }

    [Fact]
    public async Task GetAvailableSlotsAsync_Monday20240503_SolarPanels_GermanAndBronzeCustomer_AllSeller1And2Selectable()
    {
        var language = "German";
        var products = new[] { "SolarPanels" };
        var customerRating = "Bronze";
        var filterDate = new DateTime(2024, 05, 03);

        var result = await _repository.GetAvailableSlotsAsync(language, products, customerRating, filterDate, CancellationToken.None);

        var availableSlots = result.ToList();
        Assert.Equal(3, availableSlots.Count);
        Assert.Contains(availableSlots, slot => slot.StartDate == new DateTime(2024, 05, 03, 10, 30, 00) && slot.AvailableCount == 1);
        Assert.Contains(availableSlots, slot => slot.StartDate == new DateTime(2024, 05, 03, 11, 00, 00) && slot.AvailableCount == 1);
        Assert.Contains(availableSlots, slot => slot.StartDate == new DateTime(2024, 05, 03, 11, 30, 00) && slot.AvailableCount == 1);
    }

    [Fact]
    public async Task GetAvailableSlotsAsync_Tuesday20240504_SolarPanelsAndHeatpumps_GermanAndGoldCustomer_Seller2FullyBooked()
    {
        var language = "German";
        var products = new[] { "SolarPanels", "Heatpumps" };
        var customerRating = "Gold";
        var filterDate = new DateTime(2024, 05, 04);

        var result = await _repository.GetAvailableSlotsAsync(language, products, customerRating, filterDate, CancellationToken.None);

        Assert.Empty(result);
    }

    [Fact]
    public async Task GetAvailableSlotsAsync_Tuesday20240504_Heatpumps_EnglishAndSilverCustomer_Seller2FullyBooked()
    {
        var language = "English";
        var products = new[] { "Heatpumps" };
        var customerRating = "Silver";
        var filterDate = new DateTime(2024, 05, 04);

        var result = await _repository.GetAvailableSlotsAsync(language, products, customerRating, filterDate, CancellationToken.None);

        var availableSlots = result.ToList();
        Assert.Single(availableSlots);
        Assert.Contains(availableSlots, slot => slot.StartDate == new DateTime(2024, 05, 04, 11, 30, 00) && slot.AvailableCount == 1);
    }

    [Fact]
    public async Task GetAvailableSlotsAsync_Monday20240504_SolarPanels_GermanAndBronzeCustomer_Seller1And2Selectable_Seller2FullyBooked()
    {
        var language = "German";
        var products = new[] { "SolarPanels" };
        var customerRating = "Bronze";
        var filterDate = new DateTime(2024, 05, 04);

        var result = await _repository.GetAvailableSlotsAsync(language, products, customerRating, filterDate, CancellationToken.None);

        var availableSlots = result.ToList();
        Assert.Single(availableSlots);
        Assert.Contains(availableSlots, slot => slot.StartDate == new DateTime(2024, 05, 04, 10, 30, 00) && slot.AvailableCount == 1);
    }
}
