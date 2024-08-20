using AppointmentSystem.Domain.Models;

namespace AppointmentSystem.Infrastructure.DataAccess;

public interface ICalendarQueryRepository
{
    Task<IEnumerable<AvailableSlot>> GetAvailableSlotsAsync(string language, string[] products, string customerRating, DateTime filterDate, CancellationToken cancellationToken);
}