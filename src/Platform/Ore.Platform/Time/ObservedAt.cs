using System;
using System.Text.Json.Serialization;
using Ore.Platform.Serialization;

namespace Ore.Platform.Time
{
    /// <summary>
    /// Момент, когда когда данные попадают в систему.
    /// Одно из трёх возмжных времён: приведений к
    /// <see cref="OccurredAt"/> и <see cref="BusinessDate"/> нет намеренно.
    /// Значение приводится к UTC при создании: часы актуальные, и смещение
    /// не несёт сведения — в отличие от <see cref="OccurredAt"/>, где время
    /// зафиксированно источник данных (то есть время по источнику).
    /// </summary>
    [JsonConverter(typeof(ObservedAtJsonConverter))]
    public readonly record struct ObservedAt
    {
        private readonly DateTimeOffset _value;

        private ObservedAt(DateTimeOffset value) => _value = value;

        /// <summary>
        /// Момент мониторинга, всегда в UTC.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Значение собрано в обход <see cref="From"/> и равно значению по
        /// умолчанию.
        /// </exception>
        public DateTimeOffset Value
        {
            get
            {
                if (_value == default)
                {
                    throw new InvalidOperationException(
                        "Момент события не задан: получено значение по умолчанию. Создавать следует через From.");
                }

                return _value;
            }
        }

        /// <summary>
        /// Создаёт момент события, приводя значение к UTC.
        /// </summary>
        /// <param name="value">Момент, когда сведение было увидено.</param>
        /// <returns>Момент события.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Значение равно значению по умолчанию: время, ни кем не установлено,
        /// принимать нельзя.
        /// </exception>
        public static ObservedAt From(DateTimeOffset value)
        {
            if (value == default)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(value),
                    "Момент наблюдения не может быть значением по умолчанию.");
            }

            return new ObservedAt(value.ToUniversalTime());
        }
    }
}
