using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Ore.Platform.Time;

namespace Ore.Platform.Serialization
{
    /// <summary>
    /// Представление момента наблюдения в JSON.
    /// Нужен потому, что значение — структура с приватным полем и
    /// свойством-читателем: без преобразователя сериализатор пишет её
    /// объектом вида <c>{"value": …}</c>, а обратно собирает ЗНАЧЕНИЕ ПО
    /// УМОЛЧАНИЮ — молча, до первого обращения к свойству.
    /// Объявлен атрибутом на самом типе, а не настройкой сериализатора:
    /// настройку забывают, атрибут ездит вместе с типом.
    /// </summary>
    public sealed class ObservedAtJsonConverter : JsonConverter<ObservedAt>
    {
        /// <inheritdoc/>
        public override ObservedAt Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
            ObservedAt.From(reader.GetDateTimeOffset());

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, ObservedAt value, JsonSerializerOptions options)
        {
            ArgumentNullException.ThrowIfNull(writer);
            writer.WriteStringValue(value.Value);
        }
    }
}
