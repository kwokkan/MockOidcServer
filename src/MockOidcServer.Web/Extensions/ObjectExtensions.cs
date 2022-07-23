using System.Text.Json;

namespace System;

public static class ObjectExtensions
{
    private static readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);

    public static string ToJson<T>(this T? value)
    {
        return JsonSerializer.Serialize(value, _jsonSerializerOptions);
    }
}
