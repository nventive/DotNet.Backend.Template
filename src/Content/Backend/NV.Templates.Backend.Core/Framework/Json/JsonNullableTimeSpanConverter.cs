using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NV.Templates.Backend.Core.Framework.Json
{
    /// <summary>
    /// <see cref="JsonConverter"/> for <see cref="TimeSpan?"/>.
    /// To be removed when the following is resolved: https://github.com/dotnet/runtime/issues/29932.
    /// </summary>
    public class JsonNullableTimeSpanConverter : JsonConverter<TimeSpan?>
    {
        /// <inheritdoc/>
        public override TimeSpan? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.String)
            {
                throw new JsonException();
            }

            return TimeSpan.ParseExact(reader.GetString(), "c", CultureInfo.InvariantCulture);
        }

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, TimeSpan? value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value!.Value.ToString("c", CultureInfo.InvariantCulture));
        }
    }
}
