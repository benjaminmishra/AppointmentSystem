using AppointmentSystem.Domain;

namespace AppointmentSystem.Infrastructure;

public interface ICalendarQueryRepository
{
    Task<IEnumerable<AvailableSlot>> GetAvaiableSlotsAsync(string language, string[] products, string customerRating, DateTime filterDate, CancellationToken cancellationToken);
}