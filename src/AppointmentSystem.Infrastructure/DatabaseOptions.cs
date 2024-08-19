using System.ComponentModel.DataAnnotations;

namespace AppointmentSystem.Infrastructure;

public class DatabaseOptions
{
    public static string Section = "Database";

    [Required]
    public string Host { get; set; }
    [Required]
    public string Port { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string User { get; set; }
    [Required]
    public string Password { get; set; }
}