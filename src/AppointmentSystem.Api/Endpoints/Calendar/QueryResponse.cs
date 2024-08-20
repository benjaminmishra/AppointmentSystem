using AppointmentSystem.Domain.Models;

namespace AppointmentSystem.Api.Endpoints.Calendar;

public record QueryResponse(IEnumerable<AvailableSlot> AvailableSlots);