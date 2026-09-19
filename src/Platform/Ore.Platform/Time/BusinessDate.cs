using System;
using System.Text.Json.Serialization;
using Ore.Platform.Serialization;

namespace Ore.Platform.Time
{
    /// <summary>
    /// День, к которому относится сведение.
    /// Несёт <see cref="DateOnly"/>, а не усечённый момент: у дня нет
    /// часового пояса. Перегрузки <c>From(DateTimeOffset)</c> не
    /// существует намеренно — преобразование потребовало бы
    /// <see cref="TimeZoneInfo"/>, а его в наблюдении нет. День либо
    /// сообщён источником, либо неизвестен.
    /// </summary>
    [JsonConverter(typeof(BusinessDateJsonConverter))]
    public readonly record struct BusinessDate
    {
        private readonly DateOnly _value;

        private BusinessDate(DateOnly value) => _value = value;

        /// <summary>
        /// День, к которому относится сведение.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Значение собрано в обход <see cref="From"/>.
        /// </exception>
        public DateOnly Value
        {
            get
            {
                if (_value == default)
                {
                    throw new InvalidOperationException(
                        "День не задан: получено значение по умолчанию. Создавать следует через From.");
                }

                return _value;
            }
        }

        /// <summary>
        /// Создаёт день, к которому относится сведение.
        /// </summary>
        /// <param name="value">День.</param>
        /// <returns>День, к которому относится сведение.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Значение равно значению по умолчанию.
        /// </exception>
        public static BusinessDate From(DateOnly value)
        {
            if (value == default)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(value),
                    "День не может быть значением по умолчанию.");
            }

            return new BusinessDate(value);
        }
    }
}
