using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using JsonException = Newtonsoft.Json.JsonException;

namespace BCinema.Application.Converters;

public class DateTimeConverter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var dateString = reader.GetString();
        if (string.IsNullOrEmpty(dateString))
        {
            throw new JsonException("DateTime string is null or empty");
        }
        
        if (dateString.Length == 10)
        {
            throw new JsonException("Date time must be specified in format yyyy-MM-dd HH:mm");
        }

        if (DateTime.TryParseExact(dateString,
                ["yyyy-MM-dd HH:mm", "yyyy-MM-ddTHH:mm"],
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var result))
        {
            return result;
        }

        throw new JsonException("Invalid datetime format. Use yyyy-MM-dd HH:mm");
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString("yyyy-MM-dd HH:mm"));
    }
}