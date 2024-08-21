using DbUp;
using System.Reflection;

namespace AppointmentSystem.Migration;

public static class DatabaseMigrator
{
    public static int Execute(string connectionString)
    {
        // Ensure database is created
        EnsureDatabase.For.PostgresqlDatabase(connectionString);

        var upgrader =
            DeployChanges.To
                .PostgresqlDatabase(connectionString)
                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                .LogToConsole()
                .Build();

        // apply upgrades only if needed
        if (!upgrader.IsUpgradeRequired())
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Database already up-to-date!");
            Console.ResetColor();
            return 0;
        }

        var result = upgrader.PerformUpgrade();

        if (!result.Successful)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(result.Error);
            Console.ResetColor();
#if DEBUG
            Console.ReadLine();
#endif
            return -1;
        }

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Success!");
        Console.ResetColor();
        return 0;
    }
}