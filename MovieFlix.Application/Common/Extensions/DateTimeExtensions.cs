using System.Text.Json;
using System.Text.Json.Serialization;

namespace MovieFlix.Application.Common.Extensions;

public static class DateTimeExtensions
{
    private static readonly TimeZoneInfo TashkentTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Tashkent");

    public static DateTime ToTashkentTime(this DateTime utcDateTime)
    {
        var safeUtc = utcDateTime.Kind == DateTimeKind.Unspecified
            ? DateTime.SpecifyKind(utcDateTime, DateTimeKind.Utc)
            : utcDateTime.ToUniversalTime();
        
        return TimeZoneInfo.ConvertTimeFromUtc(safeUtc, TashkentTimeZone);
    }

    public static DateTime? ToTashkentTime(this DateTime? utcDateTime)
        => utcDateTime.HasValue ? utcDateTime.Value.ToTashkentTime() : null;
}

public class TashkentDateTimeJsonConverter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => reader.GetDateTime();

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        var tashkentTime = value.ToTashkentTime();
        writer.WriteStringValue(tashkentTime.ToString("yyyy-MM-ddTHH:mm:ss+05:00"));
    }
}
