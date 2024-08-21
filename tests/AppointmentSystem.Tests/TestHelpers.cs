
namespace AppointmentSystem.Tests;

public static class TestHelpers
{
    public static CancellationToken CreateCancellationToken(int timeOutMilliSeconds = 30000)
    {
        return new CancellationTokenSource(timeOutMilliSeconds).Token;
    }
}