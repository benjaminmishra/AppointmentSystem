using AppointmentSystem.Domain;

namespace AppointmentSystem.Api.Endpoints.Calendar;

public record QueryResponse(IEnumerable<AvailableSlot> AvailableSlots);