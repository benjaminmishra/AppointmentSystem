namespace AppointmentSystem.MigrationRunner;

public class DatabaseOptions
{
    public const string Section = "Database";

    public string Host { get; set; } = string.Empty;

    public int Port { get; set; } = 5432;

    public string Name { get; set; } = string.Empty;

    public string User { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}