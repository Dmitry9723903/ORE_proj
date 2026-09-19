using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Ore.Platform.Time;

namespace Ore.Platform.Serialization
{
    /// <summary>
    /// Представление дня в JSON. Устанавливается строкой вида <c>2026-09-19</c>,
    /// без времени и без пояса:.
    /// </summary>
    public sealed class BusinessDateJsonConverter : JsonConverter<BusinessDate>
    {
        /// <inheritdoc/>
        public override BusinessDate Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
            BusinessDate.From(DateOnly.Parse(reader.GetString() ?? string.Empty, System.Globalization.CultureInfo.InvariantCulture));

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, BusinessDate value, JsonSerializerOptions options)
        {
            ArgumentNullException.ThrowIfNull(writer);
            writer.WriteStringValue(value.Value.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture));
        }
    }
}
