using System.Text.Json;
using System.Text.Json.Serialization;

namespace AppointmentSystem.Api;

public class DateTimeConverter : JsonConverter<DateTime>
{
    private const string DateTimeFormat = "yyyy-MM-ddTHH:mm:ss.fffZ";

    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return DateTime.Parse(reader.GetString() ?? string.Empty);
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToUniversalTime().ToString(DateTimeFormat));
    }
}
