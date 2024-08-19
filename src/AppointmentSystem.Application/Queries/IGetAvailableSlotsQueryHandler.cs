using AppointmentSystem.Domain;
using OneOf;

namespace AppointmentSystem.Application.Queries;

public interface IGetAvailableSlotsQueryHandler
{
    Task<OneOf<List<AvailableSlot>,AvaiableSlotsError>> HandleAsync(
        string language, 
        string[] products, 
        string ratings, 
        DateOnly date, 
        CancellationToken cancellationToken);
}