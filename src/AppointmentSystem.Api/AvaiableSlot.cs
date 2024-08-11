namespace AppointmentSystem.Api;

public class AvailableSlot
{
    public int SlotId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int SalesManagerId { get; set; }
    public string SalesManagerName { get; set; }
    public string[] Languages { get; set; }
    public string[] Products { get; set; }
    public string[] CustomerRatings { get; set; }
}
