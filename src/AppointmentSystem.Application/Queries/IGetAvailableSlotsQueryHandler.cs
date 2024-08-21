using AppointmentSystem.Domain.Models;
using AppointmentSystem.Domain.Errors;
using OneOf;

namespace AppointmentSystem.Application.Queries;

public interface IGetAvailableSlotsQueryHandler
{
    Task<OneOf<List<AvailableSlot>, AvailableSlotsError>> HandleAsync(
        string language,
        string[] products,
        string rating,
        DateOnly date,
        CancellationToken cancellationToken);
}