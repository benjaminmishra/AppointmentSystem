using System.ComponentModel.DataAnnotations;

namespace AppointmentSystem.Infrastructure;

public class DatabaseOptions
{
    public const string Section = "Database";
    
    public required string Host { get; set; }
    public required string Port { get; set; }
    public required string Name { get; set; }
    public required string User { get; set; }
    public required string Password { get; set; }
}