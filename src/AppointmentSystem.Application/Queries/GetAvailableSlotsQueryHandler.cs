using AppointmentSystem.Domain.Models;
using AppointmentSystem.Domain.Enums;
using AppointmentSystem.Domain.Errors;
using AppointmentSystem.Infrastructure.DataAccess;
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
        string rating,
        DateOnly date,
        CancellationToken cancellationToken)
    {
        if (!Enum.TryParse<Language>(language, true, out _))
            return new AvailableSlotsRequestValidationError($"{language} not supported");

        if (products.Any(p=>!Enum.TryParse<Product>(p, true, out _)))
            return new AvailableSlotsRequestValidationError("One or more products are not supported");

        if (!Enum.TryParse<Rating>(rating, true, out _))
            return new AvailableSlotsRequestValidationError($"{rating} not supported");

        try
        {
            var result = await _calendarRepository.GetAvailableSlotsAsync(language, products, rating, date.ToDateTime(TimeOnly.MinValue), cancellationToken);
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