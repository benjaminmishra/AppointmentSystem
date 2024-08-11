namespace AppointmentSystem.Api;

public class SlotsRequest
{
    public DateOnly Date { get; set; }
    public string[] Products { get; set; }
    public string Language { get; set; }
    public string Rating { get; set; }
}