using System.ComponentModel.DataAnnotations;

namespace AppointmentSystem.Api.Endpoints.Calendar;

public record QueryRequest(
    [Required]string Date, 
    [Required]string[] Products, 
    [Required]string Language, 
    [Required]string CustomerRating);