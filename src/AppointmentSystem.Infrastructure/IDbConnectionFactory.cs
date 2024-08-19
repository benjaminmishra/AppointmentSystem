using System.Data;

namespace AppointmentSystem.Infrastructure;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}