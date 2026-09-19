using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Ore.Platform.Time;

namespace Ore.Platform.Serialization
{
    /// <summary>
    /// Представление момента события в JSON.
    /// Пишет момент вместе со смещением по времени источника.
    /// </summary>
    public sealed class OccurredAtJsonConverter : JsonConverter<OccurredAt>
    {
        /// <inheritdoc/>
        public override OccurredAt Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
            OccurredAt.From(reader.GetDateTimeOffset());

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, OccurredAt value, JsonSerializerOptions options)
        {
            ArgumentNullException.ThrowIfNull(writer);
            writer.WriteStringValue(value.Value);
        }
    }
}
