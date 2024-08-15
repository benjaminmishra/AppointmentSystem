namespace AppointmentSystem.Api.Endpoints.Calendar;

public record QueryRequest(string Date, string[] Products, string Language, string CustomerRating);