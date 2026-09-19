using System;
using System.Text.Json.Serialization;
using Ore.Platform.Serialization;

namespace Ore.Platform.Time
{
    /// <summary>
    /// Момент, когда событие произошло во внешнем мире.
    /// Отличается от <see cref="ObservedAt"/> тем, что задаётся источником,
    /// а не системой. Смещение, сообщённое источником, СОХРАНЯЕТСЯ: именно оно
    /// определяет, к какому календарному дню событие относится с точки
    /// зрения источника. Момент и смещение хранятся раздельно — той же
    /// формой, какой они будут зафиксированны в БД.
    /// </summary>
    [JsonConverter(typeof(OccurredAtJsonConverter))]
    public readonly record struct OccurredAt
    {
        private readonly DateTime _instantUtc;
        private readonly short _offsetMinutes;
        private readonly bool _isSet;

        private OccurredAt(DateTime instantUtc, short offsetMinutes)
        {
            _instantUtc = instantUtc;
            _offsetMinutes = offsetMinutes;
            _isSet = true;
        }

        /// <summary>
        /// Момент события вместе со смещением по часовому поясу источника.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Значение собрано в обход <see cref="From"/>.
        /// </exception>
        public DateTimeOffset Value
        {
            get
            {
                EnsureSet();
                return new DateTimeOffset(_instantUtc, TimeSpan.Zero)
                    .ToOffset(TimeSpan.FromMinutes(_offsetMinutes));
            }
        }

        /// <summary>
        /// Момент события в UTC. По нему события сравниваются и
        /// упорядочиваются, когда смещение по времни источника не важно.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Значение собрано в обход <see cref="From"/>.
        /// </exception>
        public DateTime InstantUtc
        {
            get
            {
                EnsureSet();
                return _instantUtc;
            }
        }

        /// <summary>
        /// Смещение по времени, сообщённое источником.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Значение собрано в обход <see cref="From"/>.
        /// </exception>
        public TimeSpan Offset
        {
            get
            {
                EnsureSet();
                return TimeSpan.FromMinutes(_offsetMinutes);
            }
        }

        /// <summary>
        /// Создаёт момент события, сохраняя смещение по времени источника.
        /// </summary>
        /// <param name="value">Момент, сообщённый источником.</param>
        /// <returns>Момент события.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Значение равно значению по умолчанию.
        /// </exception>
        public static OccurredAt From(DateTimeOffset value)
        {
            if (value == default)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(value),
                    "Момент события не может быть значением по умолчанию.");
            }

            return new OccurredAt(value.UtcDateTime, (short)value.Offset.TotalMinutes);
        }

        private void EnsureSet()
        {
            if (!_isSet)
            {
                throw new InvalidOperationException(
                    "Момент события не задан: получено значение по умолчанию. Создавать следует через From.");
            }
        }
    }
}
