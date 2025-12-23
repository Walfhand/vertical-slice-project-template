using System.Text.Json;
using System.Text.Json.Serialization;

namespace Engine.Web;

public sealed class FlexibleBooleanJsonConverter : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert) => typeToConvert == typeof(bool) || typeToConvert == typeof(bool?);

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options) =>
        typeToConvert == typeof(bool) ? new BooleanConverter() : new NullableBooleanConverter();

    private sealed class BooleanConverter : JsonConverter<bool>
    {
        public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => ReadBoolean(ref reader);

        public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options) => writer.WriteBooleanValue(value);
    }

    private sealed class NullableBooleanConverter : JsonConverter<bool?>
    {
        public override bool? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return null;
            }

            return ReadBoolean(ref reader);
        }

        public override void Write(Utf8JsonWriter writer, bool? value, JsonSerializerOptions options)
        {
            if (value.HasValue)
            {
                writer.WriteBooleanValue(value.Value);
                return;
            }

            writer.WriteNullValue();
        }
    }

    private static bool ReadBoolean(ref Utf8JsonReader reader)
    {
        return reader.TokenType switch
        {
            JsonTokenType.True => true,
            JsonTokenType.False => false,
            JsonTokenType.Number => ReadFromNumber(ref reader),
            JsonTokenType.String => ReadFromString(reader.GetString()),
            _ => throw new JsonException($"Unsupported token {reader.TokenType} for boolean conversion.")
        };
    }

    private static bool ReadFromNumber(ref Utf8JsonReader reader)
    {
        if (reader.TryGetInt64(out var value))
        {
            return value switch
            {
                0 => false,
                1 => true,
                _ => throw new JsonException("Numeric booleans must be 0 or 1.")
            };
        }

        throw new JsonException("Invalid numeric value for boolean conversion.");
    }

    private static bool ReadFromString(string? value)
    {
        if (value is null)
        {
            throw new JsonException("Boolean string cannot be null.");
        }

        if (bool.TryParse(value, out var parsed))
        {
            return parsed;
        }

        if (long.TryParse(value, out var number))
        {
            return number switch
            {
                0 => false,
                1 => true,
                _ => throw new JsonException("Numeric booleans must be 0 or 1.")
            };
        }

        throw new JsonException($"Cannot convert value '{value}' to boolean.");
    }
}
