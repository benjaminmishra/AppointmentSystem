namespace AppointmentSystem.Domain.Errors;

public record AvailableSlotsError(string Message, Exception? InnerException = default);
public record AvailableSlotsRequestValidationError(string ValidationErrMsg) : AvailableSlotsError(ValidationErrMsg);
public record AvailableSlotsExceptionError(Exception InnerException) : AvailableSlotsError(InnerException.Message, InnerException);