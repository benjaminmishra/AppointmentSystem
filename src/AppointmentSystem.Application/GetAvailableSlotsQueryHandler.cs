using AppointmentSystem.Domain;
using AppointmentSystem.Infrastructure;
using OneOf;
using OneOf.Types;

namespace AppointmentSystem.Application;

public class GetAvailableSlotsQueryHandler
{
    private readonly ICalendarQueryRepository _calendarRepository;
    public GetAvailableSlotsQueryHandler(ICalendarQueryRepository calendarRepository)
    {
        _calendarRepository = calendarRepository;
    }

    public async Task<OneOf<List<AvailableSlot>,AvaiableSlotsError>> HandleAsync(string language, string[] products, string ratings, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(language))
            return new AvailableSlotsRequestValidationError("Language cannot be empty or whitespace");
        
        if(!products.Any())
            return new AvailableSlotsRequestValidationError("At least one product must be specified");
        
        if(string.IsNullOrWhiteSpace(ratings))
            return new AvailableSlotsRequestValidationError("Rating cannot be empty or whitespace");
        
        try
        {
            var result = await _calendarRepository.GetAvaiableSlotsAsync(language, products, ratings, cancellationToken);
            var availableSlots = result.ToList();
            
            if (!availableSlots.Any())
                return new AvaiableSlotsNotFoundError();
            
            return availableSlots;
        }
        catch (Exception ex)
        {
            return new AvaiableSlotsExceptionError(ex);
        }
    }
}