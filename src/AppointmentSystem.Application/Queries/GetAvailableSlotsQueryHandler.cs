using AppointmentSystem.Domain;
using AppointmentSystem.Domain.Errors;
using AppointmentSystem.Infrastructure;
using OneOf;

namespace AppointmentSystem.Application.Queries;

public class GetAvailableSlotsQueryHandler : IGetAvailableSlotsQueryHandler
{
    private readonly ICalendarQueryRepository _calendarRepository;

    public GetAvailableSlotsQueryHandler(ICalendarQueryRepository calendarRepository)
    {
        _calendarRepository = calendarRepository;
    }

    public async Task<OneOf<List<AvailableSlot>, AvailableSlotsError>> HandleAsync(
        string language,
        string[] products,
        string ratings,
        DateOnly date,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(language))
            return new AvailableSlotsRequestValidationError("Language cannot be empty or whitespace");

        if (!products.Any())
            return new AvailableSlotsRequestValidationError("At least one product must be specified");

        if (string.IsNullOrWhiteSpace(ratings))
            return new AvailableSlotsRequestValidationError("Rating cannot be empty or whitespace");

        if (date == DateOnly.MinValue || date == DateOnly.MaxValue)
            return new AvailableSlotsRequestValidationError("Date is invalid");

        try
        {
            var result = await _calendarRepository.GetAvailableSlotsAsync(language, products, ratings, date.ToDateTime(TimeOnly.MinValue), cancellationToken);
            var availableSlots = result.ToList();

            if (!availableSlots.Any())
                return new AvailableSlotsNotFoundError();

            return availableSlots;
        }
        catch (Exception ex)
        {
            return new AvailableSlotsExceptionError(ex);
        }
    }
}