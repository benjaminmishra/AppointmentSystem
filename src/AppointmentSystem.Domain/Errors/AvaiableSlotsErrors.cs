namespace AppointmentSystem.Domain;

public record AvaiableSlotsError(string Message, Exception? InnerException = default);
public record AvailableSlotsRequestValidationError(string ValidationErrMsg) : AvaiableSlotsError(ValidationErrMsg);
public record AvaiableSlotsNotFoundError() : AvaiableSlotsError("No matching slot found");
public record AvaiableSlotsExceptionError(Exception InnerException) : AvaiableSlotsError(InnerException.Message, InnerException);