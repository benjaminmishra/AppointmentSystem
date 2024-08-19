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
    public string Database { get; set; }
    [Required]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
}