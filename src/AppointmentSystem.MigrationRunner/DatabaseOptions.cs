namespace AppointmentSystem.MigrationRunner;

public class DatabaseOptions
{
    public const string Section = "Database";

    public string? Host { get; set; }

    public int Port { get; set; } = 5432;

    public string? Name { get; set; }

    public string? User { get; set; }

    public string? Password { get; set; }
}